# How to deploy

## Re deplor scenario (putting this at the top for later usage):
1. You just need to overwrite all the files, but IIS/.NET Core is using them, so you need to stop the application pool. 
2. Click on the app pool in IIS, right click and stop, overwrite the files and recicle the app pool the same way as you stoped it.

## Server pre-requirements

These needs to be installed on the VM in order to run the application:
1. https://dotnet.microsoft.com/download/dotnet-core/3.1
![alt text](https://i.ibb.co/4S5yjkw/1.png "")
2. Once installed check in CMD or powershell with the dotnet --info the runtime installed:
![alt text](https://i.ibb.co/Xz9pbBS/2.png "")

## Website creation in PLESK

1. Once you logged in to plesk, you should be able to create a new site (in our case we will need 2 sites, one for the shop and one for the STS).
2. ![alt text](https://i.ibb.co/Z8v56dh/3.png "")
3. Don't forget to check the SSL option with let's encript, this does the HTTPS thing for the website.
4. ![alt text](https://i.ibb.co/tPSwCHn/4.png "")
5. Click ok and we're good to go, now if you navigate to the site url which you created you should see something like this:
6. ![alt text](https://i.ibb.co/sQWNq82/5.png "")
7. Meaning that the site is created, now if you RDP to the server, you should find the newly created websites folder somewhere here C:\Inetpub\vhosts\apit1.doublecount.cloud   
8. In my case it looks like this:
![alt text](https://i.ibb.co/WVShwvB/6.png "")
9. So far so good, now let's setup IIS on the server for the website.

## IIS Setup.

1. Hit windows button on the VM and start writing inetmgr and then open up IIS.
![alt text](https://i.ibb.co/TBncMCV/7.png "")
2. Navigate the application pools and you should see your newly created website's application pool:
![alt text](https://i.ibb.co/myfK5rN/8.png "")
3. Now we need to do some tweaks with it, it was created for normal .NET websites not for .net core.
4. Right click on the application pool and navigate to basic settings.
![alt text](https://i.ibb.co/SQGZ3Bx/9.png "")
5. We should swicth to No Managed code 
6.![alt text](https://i.ibb.co/kJqrwgs/10.png "")
7. Now the tricky part, PLESK creates a windows user for each website, and they are not having access rights to run the .net core runtime, we need to give execture rights to the newly created websites user.
8. We cab obtain the username from the application pool list, from this column:
9.![alt text](https://i.ibb.co/BNbXdcS/11.png "")
10. Then we need to navigate to : *C:\Program Files\dotnet*
11. Right click in the folder, go to properties, then security tab and within the tab edit. 
12. A new window should open up then add new user (as our user is not listed by default there)
12.![alt text](https://i.ibb.co/7znJZ39/12.png "")
13. Once you typede the username, click on check and then add the user.
14. It should show up in the list, add full access control to that user.
12.![alt text](https://i.ibb.co/TRtjwBP/13.png "")
13. Give okay for all of the dialogs, with this we're done with the iis setup.

14. Also ensure full rights for the IIS-user on folders
    > C:\InetPub\vhost\[domain]\[sts.subdomain]  
    > C:\InetPub\vhost\[domain]\[my.subdomain]

## Website publish.

1. This is probably similar to the desktop versions.
2. Right click on the project you want to publish and click on publish
![alt text](https://i.ibb.co/0MDqPP0/14.png "")
3. If the project has publish setup already you should see something like this. 
![alt text](https://i.ibb.co/dcsKsbt/15.png "")
4. Otherwise it will popup the new publish wizard ( if now you can click on new), choose the folder option:
![alt text](https://i.ibb.co/2y84GFx/16.png "")
5). Select the location you want to deploy to:
![alt text](https://i.ibb.co/jM2SXVb/17.png "")
6. Once you're done, you can hit publish:
![alt text](https://i.ibb.co/s5GJbX1/18.png "")
7. Once it's finished, you can see the progress on the bottom we're the build actions are shown, you can click on the folder location.
![alt text](https://i.ibb.co/fkT9ccZ/19.png "")
8. Zip the content and copy it to the file server:
![alt text](https://i.ibb.co/yPTvdxN/20.png "")
9. Then replace the content within the folder on the server, the folder PLESK created for IIS.
![alt text](https://i.ibb.co/0n5N4yX/21.png "")

## Troubleshooting.

1. If you see errors, check the event logs with event viewerr:
2. https://www.howtogeek.com/123646/htg-explains-what-the-windows-event-viewer-is-and-how-you-can-use-it/
3. The same user may have problems reading/writing files within the directory, you can use the same technique described in the IIS setup section, for giving enough rights to the user.
4. Reply URL for the MS login needs to be set up in the Active Directory, currently we're using my tenant, probably we will need a new one for that, but we can use mine that's not a problem, for that I will need the production DNS name.
5. Both appsettings files, in both webshop and sts needs to be changed, there are URL which aren't going to be correct for the production site. 
6. The same goes with the email settings.
![alt text]( https://i.ibb.co/1zdBLz6/22.png "")
![alt text]( https://i.ibb.co/GF3trs6/23.png "")











