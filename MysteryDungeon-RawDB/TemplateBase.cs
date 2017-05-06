using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace MysteryDungeon_RawDB
{
    public abstract class TemplateBase
    {
        [Browsable(false)]
        public StringBuilder Buffer { get; set; }

        [Browsable(false)]
        public StringWriter Writer { get; set; }

        public TemplateBase()
        {
            Buffer = new StringBuilder();
            Writer = new StringWriter(Buffer);
        }

        public abstract void Execute();

        // Writes the results of expressions like: "@foo.Bar"
        public virtual void Write(object value)
        {
            // Don't need to do anything special
            // Razor for ASP.Net does HTML encoding here.
            WriteLiteral(value);
        }

        public virtual void WriteAttribute(object value)
        {
            // Don't need to do anything special
            // Razor for ASP.Net does HTML encoding here.
            WriteLiteral(value);
        }

        public virtual void Write()
        {
            WriteLiteral("");
        }

        // Writes literals like markup: "<p>Foo</p>"
        public virtual void WriteLiteral(object value)
        {
            Buffer.Append(value);
        }
    }
}
