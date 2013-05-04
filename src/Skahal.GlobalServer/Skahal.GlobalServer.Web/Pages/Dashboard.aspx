<%@ Page Language="C#" Inherits="Skahal.GlobalServer.Web.Dashboard" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>Global Server Dashboard for <%=GameName%></title>
	<style type="text/css" title="currentStyle">
		@import "media/css/demo_page.css"; 
		@import "media/css/demo_table.css";
	</style>
	 <link type="text/css" rel="stylesheet" href="http://jqueryui.com/themes/base/ui.all.css" />
	<script type="text/javascript" language="javascript" src="media/js/jquery.js"></script>
	<script type="text/javascript" language="javascript" src="media/js/jquery.dataTables.js"></script>
	<script type="text/javascript" charset="utf-8">
		$(document).ready(function() {
			$('#gamesTable').dataTable();
			$('#playersTable').dataTable();
		} );
	</script>
</head>
<body>
<form id="form1" runat="server">
	<h2>Server history</h2>			
	<table cellpadding="0" cellspacing="0" border="0" class="display" width="100%">
		<thead>
			<tr>
				<th>Players</th> 
				<th>Games</th>
				<th>Ended games</th>
				<th>Quit games</th>
				<th>Lost connection games</th>
				<th>Max simultaneous games</th>
				<th>Requests</th>
				<th>iPod devices</th>
				<th>iPhone devices</th>
				<th>iPad devices</th> 
				<th>Mac devices</th>
				<th>Windows devices</th>
				<th>Android devices</th>
				<th>Web devices</th> 
				<th>Editor devices</th> 
			</tr> 
		</thead>		 
		<tbody>
			<tr>
				<td><%= Statistics.TotalPlayersCount %></td>
				<td><%= Statistics.TotalGamesCount %></td>
				<td><%= Statistics.TotalEndedGamesCount %></td>
				<td><%= Statistics.TotalQuitGamesCount %></td>
				<td><%= TotalLostConnectionsGames %></td>
				<td><%= Statistics.MaxSimultaneousGamesCount %></td>
				<td><%= Statistics.TotalRequestsCount %></td>
				<td><%= Statistics.TotaliPodDevicesCount %></td>
				<td><%= Statistics.TotaliPhoneDevicesCount %></td>
				<td><%= Statistics.TotaliPadDevicesCount %></td>
				<td><%= Statistics.TotalMacDevicesCount %></td>
				<td><%= Statistics.TotalWindowsDevicesCount %></td>
				<td><%= Statistics.TotalAndroidDevicesCount %></td>
				<td><%= Statistics.TotalWebDevicesCount %></td>
				<td><%= Statistics.TotalEditorDevicesCount %></td>
			</tr>
		</tbody>
	</table>	
	<h2>Current server state</h2>			
	<table cellpadding="0" cellspacing="0" border="0" class="display" width="100%">
		<thead>
			<tr>
				<th>Current available players</th>
				<th>Current games</th>
				<th>Current requests</th>
				<th>State</th>
			</tr> 
		</thead>		 
		<tbody>
			<tr>
				<td><%= Server.AvailablePlayersCount %></td>
				<td><%= Server.GamesCount %></td>
				<td><%= Server.RequestsCount %></td>
				<td><%= Server.State %></td>
			</tr>
		</tbody>
	</table>	
	<br/><hr/><br/>		
	<h2>Games</h2>			
	<table cellpadding="0" cellspacing="0" border="0" class="display" id="gamesTable" width="100%">
		<thead>
			<tr>
				<th>ID</th>
				<th>Start date</th>
				<th>Host player</th>	
				<th>Guess player</th>
				<th>Communication type</th>
			</tr>
		</thead>		
		<tbody>
			<asp:Repeater runat="server" id="gamesRepeater">
			  <ItemTemplate> 
				<tr>
					<td><%# DataBinder.Eval(Container.DataItem, "Id") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "StartDate") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "HostPlayer.Name") %> (<%# DataBinder.Eval(Container.DataItem, "HostPlayer.State") %>)</td>
					<td><%# DataBinder.Eval(Container.DataItem, "GuessPlayer.Name") %> (<%# DataBinder.Eval(Container.DataItem, "GuessPlayer.State") %>)</td>
					<td><%# DataBinder.Eval(Container.DataItem, "CommunicationType") %></td>
				</tr>
			  </ItemTemplate>
			</asp:Repeater> 
		</tbody>
	</table>  
	<br/><hr/><br/>	 
	<h2>Players</h2>			
	<table cellpadding="0" cellspacing="0" border="0" class="display" id="playersTable" width="100%">
		<thead>
			<tr>
				<th>ID</th>
				<th>Name</th>
				<th>IP</th> 
				<th>Device</th>
				<th>Game version</th>
				<th>State</th> 
				<th>Last activity date</th> 
			</tr> 
		</thead>		
		<tbody>
			<asp:Repeater runat="server" id="playersRepeater">
			  <ItemTemplate> 
				<tr>
					<td><%# DataBinder.Eval(Container.DataItem, "Id") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "IP") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "Device") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "GameVersion") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "State") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "LastActivityDate") %></td>	
				</tr>
			  </ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
</form>
<%= LastErrorMessage %>
</body>
</html>