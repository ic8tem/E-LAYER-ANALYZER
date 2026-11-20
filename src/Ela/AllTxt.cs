// E-LAYER ANALYZER - lettura dei file ALL.TXT di JTDX e WSJT-X (decodifiche) come sorgente di nubi per la verifica delle catene
// (BENCHMARK 50 MHz). Proposta dell'autore (IC8TEM, 20/11/2026): le decodifiche registrano tutto quello che la stazione ha
// sentito, non solo i QSO scelti dall'operatore. Ogni decodifica diventa uno "spot": la stazione che ascolta (riconosciuta dalle
// proprie righe in trasmissione, "CQ NOMINATIVO LOCATOR" o "DX NOMINATIVO ...") e la stazione sentita; un messaggio "A B rapporto"
// diventa anche il collegamento fra due terzi A e B, se i locator di entrambi sono noti (dal messaggio, da altre decodifiche
// o da CALL3). Una sola decodifica per minuto e per coppia.
// Formati: JTDX "20250601_082530 -16  0.1 1464 ~ IV3DXW TA4SSK RR73" con la frequenza nelle righe "... 50.313 MHz  FT8";
//          WSJT-X "250601_082530    50.313 Rx FT8    -16  0.1 1464 IV3DXW TA4SSK RR73".
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ELA
{
    public static class AllTxt
    {
        static readonly Regex rxGrid = new Regex(@"^[A-R]{2}[0-9]{2}$");
        static readonly Regex rxCall = new Regex(@"^([A-Z0-9]{1,3}[0-9][A-Z0-9]{0,4}[A-Z](/[A-Z0-9]+)?|[A-Z0-9]+/[A-Z0-9]{1,3}[0-9][A-Z0-9]*[A-Z])$");
        static readonly Regex rxJtdxHead = new Regex(@"^(\d{8}_\d{6})\s+([\d.]+) MHz\s");
        static readonly Regex rxWsjt = new Regex(@"^(\d{6}_\d{6})\s+([\d.]+)\s+(Rx|Tx)\s+(\S+)\s+(-?\d+)\s+(-?[\d.]+)\s+(\d+)\s+(.*)$");
        static readonly string[] CqWords = { "DX", "EU", "NA", "SA", "AS", "AF", "OC", "JA", "TEST", "WW", "POTA", "SOTA" };

        // proprietario del file (nominativo e locator) dalle righe in trasmissione
        public static void Owner(string path, out string call, out string grid)
        {
            Dictionary<string, int> n = new Dictionary<string, int>();
            Dictionary<string, string> g = new Dictionary<string, string>();
            int k = 0;
            foreach (string line in File.ReadLines(path))
            {
                string msg = null;
                int i = line.IndexOf("Transmitting", StringComparison.Ordinal);
                if (i >= 0) { int c = line.IndexOf(':', i); if (c > 0) msg = line.Substring(c + 1); }
                else { Match m = rxWsjt.Match(line); if (m.Success && m.Groups[3].Value == "Tx") msg = m.Groups[8].Value; }
                if (msg == null) continue;
                string[] t = msg.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string own = null, og = null;
                if (t.Length >= 2 && t[0] == "CQ")
                {
                    int j = 1; if (t.Length > 2 && Array.IndexOf(CqWords, t[1]) >= 0) j = 2;
                    own = t[j]; if (t.Length > j + 1 && rxGrid.IsMatch(t[j + 1])) og = t[j + 1];
                }
                else if (t.Length >= 2) { own = t[1]; if (t.Length > 2 && rxGrid.IsMatch(t[2])) og = t[2]; }
                if (own == null || !rxCall.IsMatch(own)) continue;
                int v; n.TryGetValue(own, out v); n[own] = v + 1;
                if (og != null) g[own] = og;
                if (++k > 3000) break;
            }
            call = n.Count > 0 ? n.OrderByDescending(x => x.Value).First().Key : "";
            string gg; grid = g.TryGetValue(call, out gg) ? gg : "";
        }

        // decodifiche fra minMHz e maxMHz come spot; ownGrid vuoto = dalle righe in trasmissione, poi CALL3, poi la propria stazione
        public static List<Spot> Read(string path, double minMHz, double maxMHz, out string owner)
        {
            string oc, og;
            Owner(path, out oc, out og);
            owner = oc;
            if (og.Length < 4) og = CallDb.Lookup(oc) ?? (string.Equals(oc, Station.Call, StringComparison.OrdinalIgnoreCase) ? Station.Loc : "");
            List<Spot> res = new List<Spot>();
            double ola, olo;
            if (og.Length < 4 || !Geo.LocToLatLon(og, out ola, out olo)) return res;
            Dictionary<string, string> grids = new Dictionary<string, string>();
            HashSet<string> seen = new HashSet<string>();
            List<string[]> pairs = new List<string[]>();      // {tempo, mittente, destinatario, MHz} per il secondo passaggio
            double fmhz = 0;
            foreach (string line in File.ReadLines(path))
            {
                string ts, msg; double f;
                Match h = rxJtdxHead.Match(line);
                if (h.Success && line.IndexOf("Transmitting", StringComparison.Ordinal) < 0) { double.TryParse(h.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out fmhz); continue; }
                if (line.IndexOf("Transmitting", StringComparison.Ordinal) >= 0) continue;
                Match w = rxWsjt.Match(line);
                if (w.Success)
                {
                    if (w.Groups[3].Value != "Rx") continue;
                    ts = "20" + w.Groups[1].Value; double.TryParse(w.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out f); msg = w.Groups[8].Value;
                }
                else
                {
                    int tl = line.IndexOf('~');
                    if (tl < 0 || line.Length < 15) continue;
                    ts = line.Substring(0, 15); f = fmhz; msg = line.Substring(tl + 1);
                }
                if (f < minMHz || f > maxMHz) continue;
                string[] t = msg.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Length > 0 && (t[t.Length - 1] == "^" || t[t.Length - 1] == "*" || t[t.Length - 1] == "a1" || t[t.Length - 1] == "a2")) Array.Resize(ref t, t.Length - 1);
                if (t.Length < 2) continue;
                string snd, rcv = null, g = null;
                if (t[0] == "CQ")
                {
                    int j = 1; if (t.Length > 2 && Array.IndexOf(CqWords, t[1]) >= 0) j = 2;
                    if (j >= t.Length) continue;
                    snd = t[j]; if (t.Length > j + 1 && rxGrid.IsMatch(t[j + 1])) g = t[j + 1];
                }
                else { rcv = t[0]; snd = t[1]; if (t.Length > 2 && rxGrid.IsMatch(t[2])) g = t[2]; }
                if (snd.StartsWith("<") || !rxCall.IsMatch(snd)) continue;
                if (rcv != null && (rcv.StartsWith("<") || !rxCall.IsMatch(rcv))) rcv = null;
                if (g != null) grids[snd] = g;
                pairs.Add(new[] { ts, snd, rcv ?? "", f.ToString("R", CultureInfo.InvariantCulture) });
            }
            Func<string, string> gridOf = c => { string x; return grids.TryGetValue(c, out x) ? x : (CallDb.Lookup(c) ?? ""); };
            foreach (string[] p in pairs)
            {
                DateTime dt;
                if (!DateTime.TryParseExact(p[0], "yyyyMMdd_HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dt)) continue;
                double f = double.Parse(p[3], CultureInfo.InvariantCulture);
                string minute = dt.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
                string sg = gridOf(p[1]);
                double sla, slo;
                if (sg.Length < 4 || !Geo.LocToLatLon(sg, out sla, out slo)) continue;
                // la stazione che ascolta sente il mittente
                if (seen.Add(minute + "|" + p[1]))
                    res.Add(new Spot { Utc = dt, FreqKHz = f * 1000, Spotter = oc, LocSpotter = og, DxCall = p[1], LocDx = sg, LatS = ola, LonS = olo, LatD = sla, LonD = slo, DigiMode = "FT8", Comment = "ALL.TXT" });
                // collegamento fra due terzi
                if (p[2].Length == 0 || p[2] == oc) continue;
                string rg = gridOf(p[2]);
                double rla, rlo;
                if (rg.Length < 4 || !Geo.LocToLatLon(rg, out rla, out rlo)) continue;
                if (seen.Add(minute + "|" + p[2] + "|" + p[1]))
                    res.Add(new Spot { Utc = dt, FreqKHz = f * 1000, Spotter = p[2], LocSpotter = rg, DxCall = p[1], LocDx = sg, LatS = rla, LonS = rlo, LatD = sla, LonD = slo, DigiMode = "FT8", Comment = "ALL.TXT" });
            }
            return res;
        }
    }
}
