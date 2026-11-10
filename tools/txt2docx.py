# E-LAYER ANALYZER - converte un documento nel formato di testo del programma (TITLE|, SUB|, H1|, H2|, P|, TAG|tipo|, EQ|, LI|, SRC|)
# in un file Word. Uso: python3 tools/txt2docx.py data/ela_documento_tecnico.txt data/ela_documento_tecnico.docx
import sys
from docx import Document
from docx.shared import Pt, RGBColor, Cm
from docx.enum.text import WD_ALIGN_PARAGRAPH

src, out = sys.argv[1], sys.argv[2]
import os, re as _re
_m = _re.search(r'_([a-z]{2}(?:-[a-z]{2})?)\.txt$', os.path.basename(src))
_lang = _m.group(1) if _m else 'it'
# etichette FONTE/RILIEVO/SCELTA tradotte: dal file data/lingue/ela-<codice>.txt della lingua del documento
TAGNAME = {}
_lf = os.path.join(os.path.dirname(os.path.abspath(src)), 'lingue', 'ela-' + _lang + '.txt')
if os.path.isfile(_lf):
    for _l in open(_lf, encoding='utf-8-sig'):
        _k, _t, _v = _l.rstrip('\n').partition('\t')
        if _t and _k in ('FONTE', 'RILIEVO', 'SCELTA'): TAGNAME[_k] = _v.strip()
d = Document()
for s in d.sections:
    s.left_margin = s.right_margin = Cm(2.2); s.top_margin = s.bottom_margin = Cm(2)
st = d.styles['Normal']; st.font.name = 'Calibri'; st.font.size = Pt(11)
COL = {'FONTE': RGBColor(0x1F, 0x5F, 0xAD), 'RILIEVO': RGBColor(0x2E, 0x7D, 0x32), 'SCELTA': RGBColor(0xB4, 0x53, 0x09)}
tab = None   # tabella in corso (righe TAB|cella;cella;...; la prima e' l'intestazione)
for raw in open(src, encoding='utf-8'):
    raw = raw.rstrip('\n')
    if not raw.strip(): continue
    kind, _, text = raw.partition('|')
    kind = kind.strip().upper()
    if kind == 'TAB':
        cells = [c.strip() for c in text.split(';')]
        if tab is None:
            tab = d.add_table(rows=0, cols=len(cells)); tab.style = 'Table Grid'; head = True
        else: head = False
        row = tab.add_row().cells
        for i, c in enumerate(cells[:len(row)]):
            row[i].text = c
            if head or i == 0:
                for r in row[i].paragraphs[0].runs: r.bold = True
        continue
    if tab is not None:
        tab = None; d.add_paragraph()
    if kind == 'TITLE':
        p = d.add_paragraph(); r = p.add_run(text); r.bold = True; r.font.size = Pt(22); r.font.color.rgb = RGBColor(24, 32, 72)
    elif kind == 'SUB':
        p = d.add_paragraph(); r = p.add_run(text); r.italic = True; r.font.color.rgb = RGBColor(100, 100, 120)
    elif kind == 'H1':
        d.add_heading(text, level=1)
    elif kind == 'H2':
        d.add_heading(text, level=2)
    elif kind == 'TAG':
        t, _, body = text.partition('|')
        p = d.add_paragraph(); r = p.add_run(TAGNAME.get(t.strip().upper(), t.strip().upper()) + '  '); r.bold = True; r.font.size = Pt(9); r.font.color.rgb = COL.get(t.strip().upper(), COL['FONTE'])
        p.add_run(body)
    elif kind == 'EQ':
        p = d.add_paragraph(); p.alignment = WD_ALIGN_PARAGRAPH.CENTER; r = p.add_run(text); r.italic = True; r.font.name = 'Cambria Math'; r.font.size = Pt(12)
    elif kind == 'LI':
        d.add_paragraph(text, style='List Bullet')
    elif kind == 'SRC':
        p = d.add_paragraph(text); p.paragraph_format.left_indent = Cm(0.8); p.paragraph_format.first_line_indent = Cm(-0.8)
    else:
        d.add_paragraph(text)
d.save(out)
print('scritto', out)
