# TeamviewerExporter
Exports the TeamViewer connection list into a csv file (compatible with RemoteDesktopManager)

# Export
It exports two files into the program directory - so make sure your user has write permissions on it:
- original.csv - 1:1 the information which are received from teamviewer api
- rdm.csv - RemoteDesktopManager compatible csv format for importing them into any folder (https://remotedesktopmanager.com/)

# Configuration
Open the program .config file and add your Authorization token from your teamviewer account.

## AuthorizationToken
Token from your teamviewer account which should be used. No username / password credentials are needed here. Please google how to create one.

## BaseUrl
Entry url for the teamviewer api.

## DevicesUrlPart
Address part in combination with BaseUrl which needs to be used to fetch all devices from the contact list.

## GroupsUrlPart
Same as DevicesUrlPart but for the contact list groups.

## ImportDefaultPassword
Only needed for rdm.csv - if set it adds this password to the import csv. If not set there will be no password on importing the csv.
