<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Theme="Default" %>

<%@ Register Src="~/MBeanUI.ascx" TagPrefix="uc" TagName="MBeanUI" %>
<%@ Register Assembly="App_Code" Namespace="Controls" TagPrefix="ucc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
</head>
<body>
	<form id="form1" runat="server">
		<%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
		<ucc:MBeanServerProxy ID="proxy" runat="server" ServiceUrl="tcp://localhost:1234/MBeanServer.tcp" />
		<asp:MultiView ID="view" runat="server" ActiveViewIndex="0">
		    <asp:View ID="browse" runat="server">
		        <asp:DropDownList ID="beanList" runat="server" EnableViewState="false"/>
		        <asp:Button ID="selectButton" runat="server" Text="Wybierz" OnClick="ShowDetails" />
		    </asp:View>
		    <asp:View ID="details" runat="Server">
		        <ucc:MBeanUI2 ID="MBeanUI" runat="server" MBeanServerProxyID="proxy"/>
		        <asp:Button ID="returnButton" runat="server" Text="Powrót" CommandName="PrevView" />
		    </asp:View>
		</asp:MultiView>		
		<%--<div>
			<uc:MBeanUI runat="server" ID="sampleMBeanUI" ObjectName=":type=SameThreadRunner,name=SameThread" MBeanServerProxyID="proxy" />						
		</div>--%>		
		<%--<ucc:MBeanUI2 runat="server" ObjectName=":type=SameThreadRunner,name=SameThread" MBeanServerProxyID="proxy"/>
		<ucc:MBeanUI2 runat="server" ObjectName=":type=AsynchTaskExecutor,name=asynchTaskExecutor" MBeanServerProxyID="proxy"/>
		<ucc:MBeanUI2 runat="server" ObjectName="slidingTimeScheduler1:type=SlidingTimeSimpleSchedulerSection" MBeanServerProxyID="proxy"/>
		<ucc:MBeanUI2 runat="server" ObjectName="slidingTimeScheduler1:type=SlidingTimeTaskDefinition,name=exportSwift" MBeanServerProxyID="proxy"/>--%>
	</form>
</body>
</html>
