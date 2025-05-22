# script_block_servers
**script_block_servers** is a console tool that blocks users and servers from appearing in the server list. A GUI will be created... eventually...

## How to use
**script_block_servers** requires [RedBlocklandLoader](https://gitlab.com/Eagle517/redblocklandloader) and the [PackageAnyFunction](https://gitlab.com/Queuenard/packageanyfunction) module. This add-on packages functions relating to populating the server list / pinging servers, which are protected as they are part of the default scripts.
<br/><br/>
### dumpBlockedIPs()
Prints the currently blocked IPs by index to the console, like:
```
0: 127.0.0.1:28000
1: 255.255.255.1:28100
2: 123.456.789.1:12345
etc.
```
### addBlockedIP( string %ip )
Adds the specified IP to the IP ban list. Specified as "IP:PORT".
<br/><br/>
### removeBlockedIP( string %address )
Removes the specified IP from the IP ban list. This function will fail if the specified IP is not found.
<br/><br/>
### removeBlockedIPbyIndex( int %index )
Like the above, but takes the index of the IP instead. Use with **dumpBlockedIPs()**.
<br/><br/>
### isIPBlocked( string %ip ) -> bool
Returns TRUE if the specified IP is blocked, else FALSE.
<br/><br/>
### dumpBlockedAdmins()
Prints the currently blocked Admin user names by index to the console, like:
```
0: User12
1: User8
2: User5
etc.
```
<br/><br/>
### addBlockedAdmin( string %adminName )[^1]
Like **addBlockedIP()**, but for usernames.
<br/><br/>
### removeBlockedAdmin( string %adminName )
Like **removeBlockedIP()**, but for usernames.
<br/><br/>
### removeBlockedAdminbyIndex( int %index )
Like **removeBlockedIPbyIndex()**, but for usernames.
<br/><br/>
### isAdminBlocked( string %adminName ) -> bool
Like **isIPBlocked()**, but for usernames.
<br/><br/>
## Other functions
### exportBannedShitWhatever()
Immediately exports the admin and IP ban lists to **config/client/**.\
IPs are saved to **"bs_byIP.cs"** and admin names are saved to **"bs_byName.cs"**\
Used on game shutdown.
<br/><br/>
### isServerBlocked( ServerSO %server )
Checks the given server if it's blocked. Runs **isAdminBlocked()** and **isIPBlocked()**.
<br/><br/>
### blankServerSO( ServerSO %server )
Sets the given server's ping to 999999 so the server list filters it, and adds the server's IP to the banned IP list.
<br/><br/>
### reindexBlockedIPs()
Will iterate though the blocked IP array and replace empty indecies with the ones after it.\
Used on add-on load and by **removeBlockedIP()** and **removeBlockedIPbyIndex()**.
<br/><br/>
### reindexBlockedAdmins()
Like the above, but for the blocked admins array.\
Used on add-on load and by **removeBlockedAdmin()** and **removeBlockedAdminbyIndex()**.
<br/><br/>

[^1]: Admins will have all their servers' IPs automatically added to the IP ban list. If you want to block a specific server, add the IP instead.
