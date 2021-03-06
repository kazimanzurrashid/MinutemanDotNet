// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Minuteman.Dashboard.ActivityHub.#.ctor(System.Func`1<System.Threading.Tasks.Task`1<System.Collections.Generic.IEnumerable`1<System.String>>>,System.Func`2<System.String,System.Threading.Tasks.Task`1<System.Collections.Generic.IEnumerable`1<Minuteman.ActivityTimeframe>>>)", Justification = "Acknowledged.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Minuteman.Dashboard.ActivityHub.#EventNames()", Justification = "Async method.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Minuteman.Dashboard.ActivityHub.#Timeframes(System.String)", Justification = "Async method.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Minuteman.Dashboard.EventActivityHub.#.ctor(Minuteman.ISubscription`1<Minuteman.EventActivitySubscriptionInfo>,System.Func`1<System.Threading.Tasks.Task`1<System.Collections.Generic.IEnumerable`1<System.String>>>,System.Func`2<System.String,System.Threading.Tasks.Task`1<System.Collections.Generic.IEnumerable`1<Minuteman.ActivityTimeframe>>>)", Justification = "Acknowledged.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Minuteman.Dashboard.UserActivityHub.#.ctor(Minuteman.ISubscription`1<Minuteman.UserActivitySubscriptionInfo>,System.Func`1<System.Threading.Tasks.Task`1<System.Collections.Generic.IEnumerable`1<System.String>>>,System.Func`2<System.String,System.Threading.Tasks.Task`1<System.Collections.Generic.IEnumerable`1<Minuteman.ActivityTimeframe>>>)", Justification = "Acknowledged.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Minuteman.Dashboard.Application.#Application_Start()", Justification = "Called by asp.net framework.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "PITA")]