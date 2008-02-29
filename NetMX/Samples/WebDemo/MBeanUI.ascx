<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MBeanUI.ascx.cs" Inherits="MBeanUI" %>
<%@ Register Assembly="App_Code" Namespace="Controls" TagPrefix="uc" %>

<uc:MBeanDataSource runat="server" ID="SampleDS" ObjectName="Sample:" MBeanServerProxyID="proxy" />			

<div class="mBean">
	<div class="sectionTitle">
		MBean information
	</div>
	<div class="mBeanData">
		<table>
			<tr>
				<td>ObjectName:</td>
				<td><%=ObjectName%></td>
			</tr>
			<tr>
				<td>Description:</td>
				<td><%=MBeanDescription%></td>
			</tr>
			<tr>
				<td>Type:</td>
				<td><%=MBeanType%></td>
			</tr>
		</table>		
	</div>
	<div class="sectionTitle">
		Attributes
	</div>
	<div class="attributes">
		<asp:GridView runat="server" ID="attributes" AutoGenerateColumns="false" Width="100%" DataKeyNames="Name"
			DataSourceID="SampleDS" DataMember="Attributes">												
		</asp:GridView>								
	</div>
	<div class="sectionTitle">
		Operations
	</div>
	<div class="operations">
		<asp:GridView runat="server" ID="operations2" AutoGenerateColumns="false" Width="100%" DataKeyNames="Name"
			DataSourceID="SampleDS" DataMember="Operations">									
		</asp:GridView>
	</div>
</div>
