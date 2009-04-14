<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Theme="Default" %>

<%@ Register Assembly="NetMX.WebUI" Namespace="NetMX.WebUI.WebControls" TagPrefix="nwc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
</head>
<body>
	<form id="form1" runat="server">
		<%--<asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
		<nwc:MBeanServerProxy ID="proxy" runat="server" ServiceUrl="tcp://localhost:1234/MBeanServer.tcp" />
		<asp:ValidationSummary ID="summary" runat="server" />
		<asp:MultiView ID="view" runat="server" ActiveViewIndex="0">
		    <asp:View ID="browse" runat="server">
		        <asp:DropDownList ID="beanList" runat="server" EnableViewState="false"/>
		        <asp:Button ID="selectButton" runat="server" Text="Wybierz" OnClick="ShowDetails" CssClass="CSS_Button" />
		    </asp:View>
		    <asp:View ID="details" runat="Server">
		        <nwc:MBeanUI ID="MBeanUI" runat="server" MBeanServerProxyID="proxy" 
		        CssClass="CSS_Control" 
		        ButtonCssClass="CSS_Button" 
		        TableCellSpacing="1" 
		        TableCellPadding="1"
		        AttributeTableCssClass="Table"
		        OperationTableCssClass="Table"
		        RelationTableCssClass="Table"
		        TabularDataTableCssClass="Table"
		        SectionTitleCssClass="SectionTitle"
		        GeneralInfoCssClass="GeneralInfo"
		        GeneralInfoNameCssClass="GeneralInfoName"
		        GeneralInfoValueCssClass="GeneralInfoValue"/>
		        <asp:Button ID="returnButton" runat="server" Text="Powrót" OnClick="HideDetails" CssClass="CSS_Button"/>
		    </asp:View>
		</asp:MultiView>								
	</form>
</body>
</html>
