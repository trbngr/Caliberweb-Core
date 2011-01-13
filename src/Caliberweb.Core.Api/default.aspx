<%@ Page Language="C#" %>
<%
string originalPath = Request.Path;

// ReSharper disable AssignNullToNotNullAttribute
HttpContext.Current.RewritePath(Request.ApplicationPath, false);
// ReSharper restore AssignNullToNotNullAttribute

IHttpHandler httpHandler = new OpenRasta.Hosting.AspNet.OpenRastaIntegratedHandler();

httpHandler.ProcessRequest(HttpContext.Current);

HttpContext.Current.RewritePath(originalPath, false);
%>