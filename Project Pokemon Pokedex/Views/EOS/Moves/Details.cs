﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ProjectPokemon.Pokedex.Views.EOS.Moves
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class Details : DetailsBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("<h2>");
            
            #line 7 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("</h2>\r\n\r\n<div>\r\n    <h3>Details</h3>\r\n    <dl class=\"dl-horizontal\">\r\n        <dt" +
                    ">\r\n            ID (Decimal)\r\n        </dt>\r\n        <dd>\r\n            ");
            
            #line 16 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.ID));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n        <dt>\r\n            ID (Hex, Big Endian)\r\n        </dt>\r" +
                    "\n        <dd>\r\n            ");
            
            #line 23 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.GetIDHexBigEndian()));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n        <dt>\r\n            ID (Hex, Little Endian)\r\n        </d" +
                    "t>\r\n        <dd>\r\n            ");
            
            #line 30 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.GetIDHexLittleEndian()));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n        \r\n        <dt>\r\n            Type\r\n        </dt>\r\n       " +
                    " <dd>\r\n\t\t\t<a href=\'{page=\"eos-type-");
            
            #line 37 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.TypeID));
            
            #line default
            #line hidden
            this.Write("\"}\'>");
            
            #line 37 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.TypeName));
            
            #line default
            #line hidden
            this.Write("</a>\r\n        </dd>\r\n\r\n        <dt>\r\n            Category\r\n        </dt>\r\n       " +
                    " <dd>\r\n            ");
            
            #line 44 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Category));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n        <dt>\r\n            Base Damage\r\n        </dt>\r\n        " +
                    "<dd>\r\n            ");
            
            #line 51 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.BasePower));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n        <dt>\r\n            Base PP\r\n        </dt>\r\n        <dd>" +
                    "\r\n            ");
            
            #line 58 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.BasePP));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n        <dt>\r\n            Base Accuracy\r\n        </dt>\r\n      " +
                    "  <dd>\r\n            ");
            
            #line 65 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.BaseAccuracy));
            
            #line default
            #line hidden
            this.Write(@"
        </dd>

    </dl>
</div>
<div>
    <h3>Pokemon through Level Up</h3>
    <table Class=""table"">
        <tr>
            <th width=""10%"">
                ID (Decimal)
            </th>
            <th width=""10%"">
                ID (Hex, Big Endian)
            </th>
            <th width=""10%"">
                ID (Hex, Little Endian)
            </th>
            <th>
                Pokémon
            </th>
            <th>
                Level(s)
            </th>
        </tr>
        ");
            
            #line 90 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
 foreach (var item in Model.PokemonLevelUp)
        { 
            
            #line default
            #line hidden
            this.Write("            <tr>\r\n                <td>\r\n                    ");
            
            #line 94 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ID));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    ");
            
            #line 97 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.IDHexBigEndian));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    ");
            
            #line 100 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.IDHexLittleEndian));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    <a href=\'{page" +
                    "=\"eos-pkm-");
            
            #line 103 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ID));
            
            #line default
            #line hidden
            this.Write("\"}\'>");
            
            #line 103 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            
            #line default
            #line hidden
            this.Write("</a>\r\n                </td>\r\n                <td>\r\n                    ");
            
            #line 106 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(string.Join(", ", item.Levels)));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n            </tr>\r\n        ");
            
            #line 109 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"    </table>
</div>
<div>
    <h3>Pokemon through TM</h3>
    <table Class=""table"">
        <tr>
            <th width=""10%"">
                ID (Decimal)
            </th>
            <th width=""10%"">
                ID (Hex, Big Endian)
            </th>
            <th width=""10%"">
                ID (Hex, Little Endian)
            </th>
            <th>
                Pokémon
            </th>
        </tr>
        ");
            
            #line 129 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
 foreach (var item in Model.PokemonTM)
        { 
            
            #line default
            #line hidden
            this.Write("            <tr>\r\n                <td>\r\n                    ");
            
            #line 133 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ID));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    ");
            
            #line 136 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.IDHexBigEndian));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    ");
            
            #line 139 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.IDHexLittleEndian));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    <a href=\'{page" +
                    "=\"eos-pkm-");
            
            #line 142 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ID));
            
            #line default
            #line hidden
            this.Write("\"}\'>");
            
            #line 142 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            
            #line default
            #line hidden
            this.Write("</a>\r\n                </td>\r\n            </tr>\r\n        ");
            
            #line 145 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
 } 
            
            #line default
            #line hidden
            this.Write(@"    </table>
</div>
<div>
    <h3>Pokemon through Eggs</h3>
    <table Class=""table"">
        <tr>
            <th width=""10%"">
                ID (Decimal)
            </th>
            <th width=""10%"">
                ID (Hex, Big Endian)
            </th>
            <th width=""10%"">
                ID (Hex, Little Endian)
            </th>
            <th>
                Pokémon
            </th>
        </tr>
        ");
            
            #line 165 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
 foreach (var item in Model.PokemonEgg)
        { 
            
            #line default
            #line hidden
            this.Write("            <tr>\r\n                <td>\r\n                    ");
            
            #line 169 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ID));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    ");
            
            #line 172 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.IDHexBigEndian));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    ");
            
            #line 175 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.IDHexLittleEndian));
            
            #line default
            #line hidden
            this.Write("\r\n                </td>\r\n                <td>\r\n                    <a href=\'{page" +
                    "=\"eos-pkm-");
            
            #line 178 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ID));
            
            #line default
            #line hidden
            this.Write("\"}\'>");
            
            #line 178 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            
            #line default
            #line hidden
            this.Write("</a>\r\n                </td>\r\n            </tr>\r\n        ");
            
            #line 181 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"
 } 
            
            #line default
            #line hidden
            this.Write("    </table>\r\n</div>");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Moves\Details.tt"

private global::ProjectPokemon.Pokedex.Models.EOS.Move _ModelField;

/// <summary>
/// Access the Model parameter of the template.
/// </summary>
private global::ProjectPokemon.Pokedex.Models.EOS.Move Model
{
    get
    {
        return this._ModelField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool ModelValueAcquired = false;
if (this.Session.ContainsKey("Model"))
{
    this._ModelField = ((global::ProjectPokemon.Pokedex.Models.EOS.Move)(this.Session["Model"]));
    ModelValueAcquired = true;
}
if ((ModelValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("Model");
    if ((data != null))
    {
        this._ModelField = ((global::ProjectPokemon.Pokedex.Models.EOS.Move)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public class DetailsBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
