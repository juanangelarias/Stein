VisualStudio is the backend in net Core

Angular is the frontend in angular

I had it running but using chrome with the web security disabled. This because I had issues with the CORS configuration that will require for me to research, and I did not have the time to do it. This config needs to be done once in the app life and I just forgot how to set it.

To run chrome without the web security you should run the following command: 

chrome.exe --user-data-dir="C://Chrome dev session" --disable-web-security

The app will load the inventory and the requests in separate tables, 

The request table have a button to process each line.

When the button is clicked the app will submit the request to the API and the API will try to process it and will load again the updated inventory from the API.

Every time one request is processed the kernels inventory will be decrease by the number of kernels requested until there is not enough inventory to satisfy the request. For example, if the inventory is 100 kernels if the request is for 10 kernels the first time the button the balance will be 90 the second time 80... and so.

If need to refresh the inventory just need to re-start the back end

