﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 15.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ProjectPokemon.Pokedex.Views.EOS.Pokemon
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "15.0.0.0")]
    public partial class Details : DetailsBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("<p>\r\n\t");
            
            #line 8 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.GetCrossReferenceHtml()));
            
            #line default
            #line hidden
            this.Write(@"
</p>
<div>
    <h3>Details</h3>
	<div class=""pkm-image-inner"">
		<div data-role='commentContent' class='ipsType_normal ipsType_richText ipsContained' data-controller='core.front.core.lightboxedImages'>
			<p>
				<img class=""ipsImage"" src=""{galleryImage=""");
            
            #line 15 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.DexNumber.ToString().PadLeft(3, '0')));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 15 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("/EoS Portrait Normal\"}\" alt=\"");
            
            #line 15 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write(" Portrait\" />\r\n\t\t\t</p>\t\t\t\r\n\t\t</div>\r\n\t</div>\r\n\t<a href=\"{albumLink=\"");
            
            #line 19 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.DexNumber.ToString().PadLeft(3, '0')));
            
            #line default
            #line hidden
            this.Write(" ");
            
            #line 19 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.Name));
            
            #line default
            #line hidden
            this.Write("\"}\">See more images</a>\r\n    <dl class=\"dl-horizontal\">\r\n        <dt>\r\n          " +
                    "  ID (Decimal)\r\n        </dt>\r\n\r\n        <dd>\r\n            ");
            
            #line 26 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.ID));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n        <dt>\r\n            ID (Hex, Big Endian)\r\n        </dt>\r" +
                    "\n\r\n        <dd>\r\n            ");
            
            #line 34 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.GetIDHexBigEndian()));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n        <dt>\r\n            ID (Hex, Little Endian)\r\n        </d" +
                    "t>\r\n\r\n        <dd>\r\n            ");
            
            #line 42 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.GetIDHexLittleEndian()));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n        <dt>\r\n            Dex Number\r\n        </dt>\r\n\r\n       " +
                    " <dd>\r\n            ");
            
            #line 50 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.DexNumber));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n\r\n\r\n        <dt>\r\n            Evolves From\r\n        </dt>\r\n\r\n   " +
                    "     <dd>\r\n\t\t\t<a href=\'{page=\"eos-pkm-");
            
            #line 59 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.EvolveFromID));
            
            #line default
            #line hidden
            this.Write("\"}\'>");
            
            #line 59 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.EvolveFromName));
            
            #line default
            #line hidden
            this.Write("</a>\r\n        </dd>\r\n\r\n\r\n        <dt>\r\n            Evolution Criteria\r\n        </" +
                    "dt>\r\n\r\n        <dd>\r\n            ");
            
            #line 68 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Model.EvolveCriteria));
            
            #line default
            #line hidden
            this.Write("\r\n        </dd>\r\n    </dl>\r\n</div>\r\n\r\n<div class=\"row\">\r\n    ");
            
            #line 74 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 if (Model.Male != null)
    {
        var gender = Model.Male; 
            
            #line default
            #line hidden
            this.Write("        <div class=\"col-md-6\">\r\n            <h3>Male</h3>\r\n            <dl class=" +
                    "\"dl-horizontal\">\r\n                <dt>\r\n                    Base HP\r\n           " +
                    "     </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 85 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseHP));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n\r\n                <dt>\r\n                    Base Attack\r" +
                    "\n                </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 93 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseATK));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n\r\n                <dt>\r\n                    Base Defense" +
                    "\r\n                </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 101 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseDEF));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n\r\n                <dt>\r\n                    Base Sp. Att" +
                    "ack\r\n                </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 109 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseSPATK));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n                <dt>\r\n                    Base Sp. Defen" +
                    "se\r\n                </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 116 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseSPDEF));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n            </dl>\r\n        </div>\r\n    ");
            
            #line 120 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 }
    if (Model.Female != null)
    {
        var gender = Model.Female; 
            
            #line default
            #line hidden
            this.Write("        <div class=\"col-md-6\">\r\n            <h3>Female</h3>\r\n            <dl clas" +
                    "s=\"dl-horizontal\">\r\n                <dt>\r\n                    Base HP\r\n         " +
                    "       </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 132 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseHP));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n\r\n                <dt>\r\n                    Base Attack\r" +
                    "\n                </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 140 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseATK));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n\r\n                <dt>\r\n                    Base Defense" +
                    "\r\n                </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 148 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseDEF));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n\r\n                <dt>\r\n                    Base Sp. Att" +
                    "ack\r\n                </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 156 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseSPATK));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n                <dt>\r\n                    Base Sp. Defen" +
                    "se\r\n                </dt>\r\n\r\n                <dd>\r\n                    ");
            
            #line 163 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(gender.BaseSPDEF));
            
            #line default
            #line hidden
            this.Write("\r\n                </dd>\r\n            </dl>\r\n        </div>\r\n    ");
            
            #line 167 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 } 
            
            #line default
            #line hidden
            this.Write("</div>\r\n<div>\r\n<h3>Moves (Level Up)</h3>\r\n<table class=\"table table-striped table" +
                    "-bordered table-condensed\">\r\n    <tr>\r\n        <th>Level</th>\r\n        <th>Move<" +
                    "/th>\r\n    </tr>\r\n    ");
            
            #line 176 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 foreach (var item in Model.LevelupMoves)
    { 
            
            #line default
            #line hidden
            this.Write("        <tr>\r\n            <td>\r\n                ");
            
            #line 180 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Item1.ToString()));
            
            #line default
            #line hidden
            this.Write("\r\n            </td>\r\n            <td>\r\n\t\t\t\t<a href=\'{page=\"eos-move-");
            
            #line 183 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Item2.ID));
            
            #line default
            #line hidden
            this.Write("\"}\'>");
            
            #line 183 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Item2.Name));
            
            #line default
            #line hidden
            this.Write("</a>\r\n            </td>\r\n        </tr>\r\n    ");
            
            #line 186 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 } 
            
            #line default
            #line hidden
            this.Write("</table>\r\n</div>\r\n<div>\r\n<h3>Moves (TM)</h3>\r\n<table class=\"table table-striped t" +
                    "able-bordered table-condensed\">\r\n    <tr>\r\n        <th>Move</th>\r\n    </tr>\r\n   " +
                    " ");
            
            #line 195 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 foreach (var item in Model.TMMoves)
    { 
            
            #line default
            #line hidden
            this.Write("        <tr>\r\n            <td>\r\n\t\t\t\t<a href=\'{page=\"eos-move-");
            
            #line 199 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ID));
            
            #line default
            #line hidden
            this.Write("\"}\'>");
            
            #line 199 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            
            #line default
            #line hidden
            this.Write("</a>\r\n            </td>\r\n        </tr>\r\n    ");
            
            #line 202 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 } 
            
            #line default
            #line hidden
            this.Write("</table>\r\n</div>\r\n<div>\r\n<h3>Moves (Egg)</h3>\r\n<table class=\"table table-striped " +
                    "table-bordered table-condensed\">\r\n    <tr>            \r\n        <th>Move</th>\r\n " +
                    "   </tr>\r\n    ");
            
            #line 211 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 foreach (var item in Model.EggMoves)
    { 
            
            #line default
            #line hidden
            this.Write("        <tr>\r\n            <td>\r\n\t\t\t\t<a href=\'{page=\"eos-move-");
            
            #line 215 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.ID));
            
            #line default
            #line hidden
            this.Write("\"}\'>");
            
            #line 215 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(item.Name));
            
            #line default
            #line hidden
            this.Write("</a>\r\n            </td>\r\n        </tr>\r\n    ");
            
            #line 218 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"
 } 
            
            #line default
            #line hidden
            this.Write("</table>\r\n</div>");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "C:\Users\evanl\Git\Project-Pokemon-Pokedex\Project Pokemon Pokedex\Views\EOS\Pokemon\Details.tt"

private global::ProjectPokemon.Pokedex.Models.EOS.Pokemon _ModelField;

/// <summary>
/// Access the Model parameter of the template.
/// </summary>
private global::ProjectPokemon.Pokedex.Models.EOS.Pokemon Model
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
    this._ModelField = ((global::ProjectPokemon.Pokedex.Models.EOS.Pokemon)(this.Session["Model"]));
    ModelValueAcquired = true;
}
if ((ModelValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("Model");
    if ((data != null))
    {
        this._ModelField = ((global::ProjectPokemon.Pokedex.Models.EOS.Pokemon)(data));
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
