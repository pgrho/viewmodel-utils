/*!
 * Shipwreck.ViewModelUtils.Blazor
 */
var Shipwreck;!function(e){(e.ViewModelUtils||(e.ViewModelUtils={})).setIndeterminate=function(e,i){var n=document.getElementById(e);n?n.indeterminate=i:$(e).prop("indeterminate",i)}}(Shipwreck=Shipwreck||{}),function(e){(e.ViewModelUtils||(e.ViewModelUtils={})).toggleModal=function(e,i,n){$(e).one("hidden.bs.modal",function(){n.invokeMethodAsync("OnClosed")}).one("click",function(e){e.target===e.currentTarget&&$(e.currentTarget).modal("hide")}).modal({show:!!i,backdrop:!1}).modal(i?"show":"hide")}}(Shipwreck=Shipwreck||{});