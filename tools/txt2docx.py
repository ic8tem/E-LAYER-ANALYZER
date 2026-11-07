# E-LAYER ANALYZER - converte un documento nel formato di testo del programma (TITLE|, SUB|, H1|, H2|, P|, TAG|tipo|, EQ|, LI|, SRC|)
# in un file Word. Uso: python3 tools/txt2docx.py data/ela_documento_tecnico.txt data/ela_documento_tecnico.docx
import sys
from docx import Document
from docx.shared import Pt, RGBColor, Cm
from docx.enum.text import WD_ALIGN_PARAGRAPH

src, out = sys.argv[1], sys.argv[2]
import os, re as _re
_m = _re.search(r'_([a-z]{2})\.txt$', os.path.basename(src))
_lang = _m.group(1) if _m else 'it'
TAGNAME = {'en': {'FONTE': 'SOURCE', 'RILIEVO': 'CHECK', 'SCELTA': 'CHOICE'}, 'de': {'FONTE': 'QUELLE', 'RILIEVO': 'PRÜFUNG', 'SCELTA': 'WAHL'},
           'ja': {'FONTE': '出典', 'RILIEVO': '検証', 'SCELTA': '選択'}, 'ru': {'FONTE': 'ИСТОЧНИК', 'RILIEVO': 'ПРОВЕРКА', 'SCELTA': 'ВЫБОР'},
           'nl': {'FONTE': 'BRON', 'RILIEVO': 'CONTROLE', 'SCELTA': 'KEUZE'}}.get(_lang, {})
d = Document()
for s in d.sections:
    s.left_margin = s.right_margin = Cm(2.2); s.top_margin = s.bottom_margin = Cm(2)
st = d.styles['Normal']; st.font.name = 'Calibri'; st.font.size = Pt(11)
COL = {'FONTE': RGBColor(0x1F, 0x5F, 0xAD), 'RILIEVO': RGBColor(0x2E, 0x7D, 0x32), 'SCELTA': RGBColor(0xB4, 0x53, 0x09)}
for raw in open(src, encoding='utf-8'):
    raw = raw.rstrip('\n')
    if not raw.strip(): continue
    kind, _, text = raw.partition('|')
    kind = kind.strip().upper()
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
