﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DocumentScanner {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("DocumentScanner.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Autofill AutoIncrement fields above.
        /// </summary>
        internal static string AutofillDateButtonLabel {
            get {
                return ResourceManager.GetString("AutofillDateButtonLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ◀ Decrement.
        /// </summary>
        internal static string DecrementButtonLabel {
            get {
                return ResourceManager.GetString("DecrementButtonLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disregard.
        /// </summary>
        internal static string DocPageTrashButtonLabel {
            get {
                return ResourceManager.GetString("DocPageTrashButtonLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Increment ▶.
        /// </summary>
        internal static string IncrementButtonLabel {
            get {
                return ResourceManager.GetString("IncrementButtonLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot Increment.
        /// </summary>
        internal static string IncrementUndatedButtonText {
            get {
                return ResourceManager.GetString("IncrementUndatedButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Date.
        /// </summary>
        internal static string UndatedPreviewText {
            get {
                return ResourceManager.GetString("UndatedPreviewText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Currently Viewing.
        /// </summary>
        internal static string ViewingFullSizeDocButtonLabel {
            get {
                return ResourceManager.GetString("ViewingFullSizeDocButtonLabel", resourceCulture);
            }
        }
    }
}
