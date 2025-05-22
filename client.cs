if( isPackage( blockPlayerServers ) )
	deactivatePackage( blockPlayerServers );

$blockedIPs_count = 0;
// $blockedIPs[$blockedIPs_count] = 0;
function addBlockedIP( %ip ){
	echo( "\c2Blocking IP" SPC %ip @ "..." );
	
	if( isIPBlocked( %ip ) )
	{
		echo( "\c2IP already blocked." );
		return;
	}
	
	$blockedIPs[$blockedIPs_count++ - 1] = %ip;
}


function isIPBlocked( %ip ){
	for( %i = 0; %i < $blockedIPs_count; %i++ ){
		if( %ip $= $blockedIPs[%i] )
			return true;
	}
	
	return false;
}


function dumpBlockedIPs(){
	echo("");
	echo( "Currently blocked IPs:" );
	for( %i = 0; %i < $blockedIPs_count; %i++ ){
		echo( %i @ ":" SPC $blockedIPs[%i] );
	}
	echo( "\c5Please note that blocked admins will have their IPs automatically added to this list." );
	echo( "\c5If you want to add a specific server, add the IP instead." );
}


function removeBlockedIP( %address ){
	echo("");
	for( %i = 0; %i < $blockedIPs_count; %i++ ){
		// echo( %address SPC "$=" SPC $blockedIPs[%i] @ "?" );
		if( strMatch( %address, $blockedIPs[%i] ) )
		{
			echo( "\c4IP found, removing..." );
			$blockedIPs[%i] = "";
			reindexBlockedIPs();
			return;
		}
	}
	
	echo( "\c2IP not found, ensure you have typed it correctly and try again." );
}


function removeBlockedIPbyIndex( %index ){
	if( %index < 0 || %index > $blockedIPs_count ){
		echo( "\c2Index out of range. Please ensure %index is above or equal to 0 and below" SPC $blockedIPs_count @ "." );
		return;
	}
	
	echo( "\c4Removing..." );
	$blockedIPs[%index] = "";
	reindexBlockedIPs();
}


function reindexBlockedIPs(){
	echo("");
	echo( "\c3Reindexing blocked IPs..." );
	for( %r_index = $blockedIPs_count - 1; %r_index >= 0; %r_index-- ){
		if( strLen( $blockedIPs[ %r_index ] ) == 0 )
		{
			%was = $blockedIPs_count;
			$blockedIPs_count--;
			
			// echo( "\c2MISSING INDEX FOUND AT" SPC %r_index SPC " (" SPC %was SPC ">" SPC $BANs_count @ ")" );
			
			for( %index = %r_index; %index < $blockedIPs_count; %index++ ){
				$blockedIPs[ %index ] = $blockedIPs[ %index + 1 ];
			}
		}
	}
	
	dumpBlockedIPs();
}


$BANs_count = 0;
$BANs[$BANs_count++ - 1] = "Feet Club";
function isAdminBlocked( %adminName ){
	for( %i = 0; %i < $BANs_count; %i++ ){
		if( strMatch( %adminName, $BANs[%i] ) )
			return true;
	}
	
	return false;
}


function dumpBlockedAdmins(){
	echo("");
	echo( "Currently blocked admins:" );
	for( %i = 0; %i < $BANs_count; %i++ ){
		echo( %i @ ":" SPC $BANs[%i] );
	}
	echo( "\c5Please note that blocked admins will have their IPs automatically added to the blocked IPs list." );
	echo( "\c5If you want to add a specific server, add the IP instead." );
}


function addBlockedAdmin( %adminName ){
	echo( "\c2Blocking admin" SPC %adminName @ "..." );
	
	if( isAdminBlocked( %adminName ) )
	{
		echo( "\c2Admin already blocked." );
		return;
	}
	
	$BANs[$BANs_count++ - 1] = %adminName;
}


function removeBlockedAdmin( %adminName ){
	echo("");
	for( %i = 0; %i < $BANs_count; %i++ ){
		// echo( %adminName SPC "$=" SPC $BANs[%i] @ "?" );
		if( strMatch( %adminName, $BANs[%i] ) )
		{
			echo( "\c4Admin found, removing..." );
			$BANs[%i] = "";
			reindexBlockedAdmins();
			return;
		}
	}
	
	echo( "\c2Admin not found, ensure you have the correct name and try again." );
}


function removeBlockedAdminbyIndex( %index ){
	if( %index < 0 || %index >= $BANs_count ){
		echo( "\c2Index out of range. Please ensure %index is above or equal to 0 and below" SPC $BANs_count @ "." );
		return;
	}
	
	echo( "\c4Removing..." );
	$BANs_count[%index] = "";
	reindexBlockedAdmins();
}


function reindexBlockedAdmins(){
	echo("");
	echo( "\c3Reindexing blocked admins..." );
	for( %r_index = $BANs_count - 1; %r_index >= 0; %r_index-- ){
		if( strLen( $BANs[ %r_index ] ) == 0 )
		{
			%was = $BANs_count;
			$BANs_count--;
			
			// echo( "\c2MISSING INDEX FOUND AT" SPC %r_index SPC " (" SPC %was SPC ">" SPC $BANs_count @ ")" );
			
			for( %index = %r_index; %index < $BANs_count; %index++ ){
				$BANs[ %index ] = $BANs[ %index + 1 ];
			}
		}
	}
	
	dumpBlockedAdmins();
}


if(isFile("config/client/bs_byName.cs"))
{
	echo( "\c3Importing existing banned users list..." );
	exec("config/client/bs_byName.cs");
	reindexBlockedAdmins();
}


if(isFile("config/client/bs_byIP.cs")){
	echo( "\c3Importing existing banned IPs list..." );
	exec("config/client/bs_byIP.cs");
	reindexBlockedIPs();
}


function isServerBlocked( %server ){
	return isAdminBlocked( %server.adminName ) || isIPBlocked( getServerSOAddress( %server ) );
}


function blankServerSO( %server ){
	echo( "\c2Blanking server info (" @ getServerSOAddress( %server ) @ ")..." );
	%server.ping = 999999;
	addBlockedIP( getServerSOAddress( %server ) );
}


function scanBlocked(){
	echo("");
	echo( "\c4Scanning for bad servers..." );
	
	for (%i = 0; %i < $ServerSO_Count; %i++)
	{
		%server = $ServerSO[%i];
		echo( "\c1server:" SPC %server.adminName @ "'s" SPC %server.serverName SPC "(" @ getServerSOAddress(%server) @ ")" );
		if( isServerBlocked( %server ) ){
			blankServerSO( %server );
		}
	}
}


function getServerSOAddress( %server ){
	return %server.ip @ ":" @ %server.port;
}


function stripPort( %address ){
	%addressLocation = strPos( %address, ":" );
	%address = getSubStr( %address, 0, %addressLocation );
	
	return %address;
}


function exportBannedShitWhatever(){
	echo( "\c4Exporting banned users and IPs list..." );
	export("$BANs*", "config/client/bs_byName.cs");
	export("$blockedIPs*", "config/client/bs_byIP.cs");
}


package blockPlayerServers
{
	
	function ServerInfoSO_DisplayAll()
	{
		scanBlocked();
		
		parent::ServerInfoSO_DisplayAll();
	}
	
	
	// Just straight-up reimplementing this function. I can't prevent pinging the servers in it otherwise.
	// Lifted directly from https://github.com/Elletra/bl-decompiled/blob/main/v21/client/scripts/allClientScripts.cs
	function ServerInfoSO_StartPingAll()
	{
		echo("");
		echo("\c4Pinging Servers (custom)...");
		if ($Pref::Net::MaxSimultaneousPings <= 0)
		{
			$Pref::Net::MaxSimultaneousPings = 10;
		}
		$ServerSO_PingCount = 0;
		if ($ServerSO_Count < $Pref::Net::MaxSimultaneousPings)
		{
			%count = $ServerSO_Count;
		}
		else
		{
			%count = $Pref::Net::MaxSimultaneousPings;
		}
		for (%i = 0; %i < %count; %i++)
		{
			%server = $ServerSO[%i];
			if( isServerBlocked( %server ) ){
				echo( "\c2IP (" @ getServerSOAddress( %server ) @ ") is blocked, not pinging." );
				continue;
			}
			
			%addr = getServerSOAddress( %server );
			echo("\c1Sending ping to    IP:" @ %addr);
			pingSingleServer(%addr, %i);
			$ServerSO_PingCount = %i;
		}
	}
	
	
	function ServerInfoSO_PingNext(%slot)
	{
		%server = $ServerSO[$ServerSO_PingCount];
		if( isServerBlocked( %server ) ){
			$ServerSO_PingCount++;
			echo( "\c2IP (" @ getServerSOAddress( %server ) @ ") is blocked, not pinging." );
			return;
		}
		
		parent::ServerInfoSO_PingNext(%slot);
	}
	
	
	function onSimplePingReceived(%address, %ping, %slot)
	{
		if( isIPBlocked( %address ) ){
			echo( "\c2IP (" @ %address @ ") is blocked, ignoring (ping recieved)." );
			return;
		}
		
		parent::onSimplePingReceived(%address, %ping, %slot);
	}
	
	
	function onSimplePingTimeout(%address, %slot)
	{
		if( isIPBlocked( %address ) ){
			echo( "\c2IP (" @ %address @ ") is blocked, ignoring (timeout)." );
			return;
		}
		
		parent::onSimplePingTimeout(%address, %slot);
	}
	
	
	function onExit()
	{
		exportBannedShitWhatever();
		parent::onExit();
	}
	
};
activatepackage( blockPlayerServers );
