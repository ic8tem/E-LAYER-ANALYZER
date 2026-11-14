// E-LAYER ANALYZER - filtri degli spot per modo di propagazione (E-sporadico, F.A.I., aurora).
// Il filtro del meteor scatter e' quello di YA-MS (MsCollector), mostrato nella stessa colonna di DATA COLLECTION.
// I valori di partenza sono le regole dell'autore (IC8TEM) descritte nella sintesi delle scelte di calcolo.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using YAMS;

namespace ELA
{
    public static class Filters
    {
        public static readonly string[] BandNames = { "6m", "4m", "2m", "70cm" };
        // il F.A.I. non arriva a 432 MHz (regola dell'autore, 14/11/2026): bande del F.A.I. senza i 70 cm
        public static readonly string[] FaiBandNames = { "6m", "4m", "2m" };

        // ---- E-sporadico
        public const string EsTagsDefault = "MS AURORA AUR AUE AU TR TROPO AS ACS";
        public static string EsTags = EsTagsDefault;            // scritte <..> che escludono uno spot dall'E-sporadico
        static Regex rxNotEs = Build(EsTagsDefault, true);

        // ---- F.A.I.
        public const string FaiTagsDefault = "FAI";
        public static string FaiTags = FaiTagsDefault;          // parole che indicano uno spot di F.A.I. (anche senza <>)
        static Regex rxFai = Build(FaiTagsDefault, false);
        public static readonly HashSet<string> FaiBands = new HashSet<string>(FaiBandNames);
        public static double FaiMinutes = 24 * 60;

        // ---- aurora
        public const string AuTagsDefault = "AU AUR";   // regola dell'autore: au o aur, con o senza < >, maiuscolo o minuscolo
        public static string AuTags = AuTagsDefault;            // parole (con o senza < >) che indicano uno spot di aurora
        static Regex rxAu = Build(AuTagsDefault, false);
        public static readonly HashSet<string> AuBands = new HashSet<string>(BandNames);
        public static double AuMinutes = 120;

        public static bool Es50WindowNotExclusive = true;   // regola dell'autore: in 50 MHz il bandplan non e' esclusivo

        public static event Action Changed;
        public static void NotifyChanged() { Action a = Changed; if (a != null) a(); }

        static IEnumerable<string> Words(string s)
        {
            return (s ?? "").Split(new[] { ' ', ',', ';', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(w => w.Trim('<', '>').Trim()).Where(w => w.Length > 0);
        }

        // brackets = true: la parola vale solo tra < >; false: anche come parola isolata
        static Regex Build(string words, bool brackets)
        {
            List<string> l = Words(words).Select(Regex.Escape).ToList();
            if (l.Count == 0) return null;
            string alt = string.Join("|", l.OrderByDescending(x => x.Length));
            string pat = brackets ? @"<\s*(" + alt + @")\s*>" : @"(?<![A-Z0-9])(" + alt + @")(?![A-Z0-9])";
            return new Regex(pat, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        public static void SetEsTags(string s) { EsTags = s ?? ""; rxNotEs = Build(EsTags, true); }
        public static void SetFaiTags(string s) { FaiTags = s ?? ""; rxFai = Build(FaiTags, false); }
        public static void SetAuTags(string s) { AuTags = s ?? ""; rxAu = Build(AuTags, false); }

        public static bool NotEsTag(string comment) { Regex r = rxNotEs; return r != null && r.IsMatch(comment ?? ""); }
        public static bool AuTag(string comment) { Regex r = rxAu; return r != null && r.IsMatch(comment ?? ""); }
        public static bool FaiTag(string comment) { Regex r = rxFai; return r != null && r.IsMatch(comment ?? ""); }

        // spot da mostrare nelle linguette F.A.I. e AURORA
        public static bool IsFaiSpot(Spot s) { return (s.Prop == "FAI" || FaiTag(s.Comment)) && FaiBands.Contains(s.Band); }
        public static bool IsAuroraSpot(Spot s) { return s.IsAurora && AuBands.Contains(s.Band); }

        public static void Load(Config c)
        {
            SetEsTags(c.Get("flt_es_scritte", EsTagsDefault));
            EsModel.MinKm50 = c.GetD("flt_es_50_min_km", 450);
            Es50WindowNotExclusive = c.GetB("flt_es_50_bandplan", true);
            EsModel.Use144Rule = c.GetB("flt_es_144", true);
            EsModel.MinKm144 = c.GetD("flt_es_144_km", 900);
            EsModel.Confirm144Min = c.GetD("flt_es_144_min", 5);
            EsModel.CloudMatchKm = c.GetD("flt_es_nube_km", 300);
            EsModel.Near50Min = c.GetD("flt_es_50_min", 15);
            EsModel.Short50Km = c.GetD("flt_es_50_km", 1200);
            EsModel.MinKm70 = c.GetD("flt_es_70_min_km", 450);
            EsModel.UseFt8CloudRule = c.GetB("flt_es_ft8_nube", true);
            EsModel.Ft8CloudMin = c.GetD("flt_es_ft8_minuti", 4);
            EsModel.Ft8CloudKm = c.GetD("flt_es_ft8_km", 150);
            EsModel.UseTepRule = c.GetB("flt_es_tep", true);
            EsModel.TepInclinationDeg = c.GetD("flt_es_tep_gradi", 20);
            SetFaiTags(c.Get("flt_fai_scritte", FaiTagsDefault));
            LoadBands(c.Get("flt_fai_bande", string.Join(" ", FaiBandNames)), FaiBands);
            FaiBands.Remove("70cm");
            FaiMinutes = c.GetD("flt_fai_minuti", 24 * 60);
            SetAuTags(c.Get("flt_au_scritte", AuTagsDefault));
            LoadBands(c.Get("flt_au_bande", string.Join(" ", BandNames)), AuBands);
            AuMinutes = c.GetD("flt_au_minuti", 120);
        }

        static void LoadBands(string s, HashSet<string> set)
        {
            set.Clear();
            foreach (string w in Words(s)) if (Array.IndexOf(BandNames, w) >= 0) set.Add(w);
        }

        public static void Save(Config c)
        {
            c.Set("flt_es_scritte", EsTags);
            c.Set("flt_es_50_min_km", EsModel.MinKm50);
            c.Set("flt_es_50_bandplan", Es50WindowNotExclusive);
            c.Set("flt_es_144", EsModel.Use144Rule);
            c.Set("flt_es_144_km", EsModel.MinKm144);
            c.Set("flt_es_144_min", EsModel.Confirm144Min);
            c.Set("flt_es_nube_km", EsModel.CloudMatchKm);
            c.Set("flt_es_50_min", EsModel.Near50Min);
            c.Set("flt_es_50_km", EsModel.Short50Km);
            c.Set("flt_es_70_min_km", EsModel.MinKm70);
            c.Set("flt_es_ft8_nube", EsModel.UseFt8CloudRule);
            c.Set("flt_es_ft8_minuti", EsModel.Ft8CloudMin);
            c.Set("flt_es_ft8_km", EsModel.Ft8CloudKm);
            c.Set("flt_es_tep", EsModel.UseTepRule);
            c.Set("flt_es_tep_gradi", EsModel.TepInclinationDeg);
            c.Set("flt_fai_scritte", FaiTags);
            c.Set("flt_fai_bande", string.Join(" ", FaiBandNames.Where(FaiBands.Contains)));
            c.Set("flt_fai_minuti", FaiMinutes);
            c.Set("flt_au_scritte", AuTags);
            c.Set("flt_au_bande", string.Join(" ", BandNames.Where(AuBands.Contains)));
            c.Set("flt_au_minuti", AuMinutes);
        }

        // ripristina le regole dell'autore
        public static void ResetEs()
        {
            SetEsTags(EsTagsDefault);
            EsModel.MinKm50 = 450; Es50WindowNotExclusive = true;
            EsModel.Use144Rule = true; EsModel.MinKm144 = 900; EsModel.Confirm144Min = 5; EsModel.CloudMatchKm = 300;
            EsModel.Near50Min = 15; EsModel.Short50Km = 1200; EsModel.UseTepRule = true; EsModel.TepInclinationDeg = 20;
            EsModel.MinKm70 = 450; EsModel.UseFt8CloudRule = true; EsModel.Ft8CloudMin = 4; EsModel.Ft8CloudKm = 150;
        }
        public static void ResetFai() { SetFaiTags(FaiTagsDefault); FaiBands.Clear(); FaiBands.UnionWith(FaiBandNames); FaiMinutes = 24 * 60; }
        public static void ResetAu() { SetAuTags(AuTagsDefault); AuBands.Clear(); AuBands.UnionWith(BandNames); AuMinutes = 120; }
    }
}
