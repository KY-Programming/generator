﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KY.Generator.OData.Tests.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("KY.Generator.OData.Tests.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to // tslint:disable
        ///
        ///import { Address } from &quot;/address&quot;;
        ///import { HttpClient } from &quot;@angular/common/http&quot;;
        ///import { Injectable } from &quot;@angular/core&quot;;
        ///import { Observable } from &quot;rxjs&quot;;
        ///import { Subject } from &quot;rxjs&quot;;
        ///
        ///@Injectable({
        ///    providedIn: &quot;root&quot;
        ///})
        ///export class AddressService {
        ///    private readonly http: HttpClient;
        ///    public serviceUrl: string = &quot;&quot;;
        ///
        ///    public constructor(http: HttpClient) {
        ///        this.http = http;
        ///    }
        ///
        ///    public get(): Observable&lt;Address[]&gt; {
        ///        le [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string address_service_ts {
            get {
                return ResourceManager.GetString("address_service_ts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // tslint:disable
        ///
        ///export class Address {
        ///    public id: number;
        ///    public zipCode: number;
        ///    public city: string;
        ///    public street: string;
        ///
        ///    public constructor(init: Partial&lt;Address&gt; = undefined) {
        ///        Object.assign(this, init);
        ///    }
        ///}.
        /// </summary>
        internal static string address_ts {
            get {
                return ResourceManager.GetString("address_ts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;edmx:Edmx Version=&quot;4.0&quot; xmlns:edmx=&quot;http://docs.oasis-open.org/odata/ns/edmx&quot;&gt;
        ///  &lt;edmx:DataServices&gt;
        ///    &lt;Schema Namespace=&quot;OData.Models&quot; xmlns=&quot;http://docs.oasis-open.org/odata/ns/edm&quot;&gt;
        ///      &lt;EntityType Name=&quot;Address&quot;&gt;
        ///        &lt;Key&gt;
        ///          &lt;PropertyRef Name=&quot;Id&quot; /&gt;
        ///        &lt;/Key&gt;
        ///        &lt;Property Name=&quot;Id&quot; Type=&quot;Edm.Guid&quot; Nullable=&quot;false&quot; /&gt;
        ///        &lt;Property Name=&quot;ZipCode&quot; Type=&quot;Edm.Int32&quot; Nullable=&quot;false&quot; /&gt;
        ///        &lt;Property Name=&quot;City&quot; Type=&quot;Edm.St [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string OData {
            get {
                return ResourceManager.GetString("OData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {
        ///  &quot;version&quot;: 2,
        ///  &quot;generate&quot;: [
        ///    {
        ///      &quot;read&quot;: &quot;odata-v4&quot;,
        ///      &quot;file&quot;: &quot;Resources/OData.xml&quot;
        ///    },
        ///    {
        ///      &quot;write&quot;: &quot;angular&quot;,
        ///      &quot;service&quot;: {
        ///        &quot;propertiesToFields&quot;: true,
        ///        &quot;formatNames&quot;: true
        ///      }
        ///    }
        ///  ]
        ///}.
        /// </summary>
        internal static string odata_generator {
            get {
                return ResourceManager.GetString("odata_generator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // tslint:disable
        ///
        ///import { User } from &quot;/user&quot;;
        ///import { HttpClient } from &quot;@angular/common/http&quot;;
        ///import { Injectable } from &quot;@angular/core&quot;;
        ///import { Observable } from &quot;rxjs&quot;;
        ///import { Subject } from &quot;rxjs&quot;;
        ///
        ///@Injectable({
        ///    providedIn: &quot;root&quot;
        ///})
        ///export class uSeRService {
        ///    private readonly http: HttpClient;
        ///    public serviceUrl: string = &quot;&quot;;
        ///
        ///    public constructor(http: HttpClient) {
        ///        this.http = http;
        ///    }
        ///
        ///    public get(): Observable&lt;User[]&gt; {
        ///        let subject =  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string u_se_r_service_ts {
            get {
                return ResourceManager.GetString("u_se_r_service_ts", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // tslint:disable
        ///
        ///export class User {
        ///    public id: number;
        ///    public name: string;
        ///    public email: string;
        ///    public address: OData.Models.Address;
        ///
        ///    public constructor(init: Partial&lt;User&gt; = undefined) {
        ///        Object.assign(this, init);
        ///    }
        ///}.
        /// </summary>
        internal static string user_ts {
            get {
                return ResourceManager.GetString("user_ts", resourceCulture);
            }
        }
    }
}
