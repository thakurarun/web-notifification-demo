# important notes

> angular service worker only supported on https or localhost (127.0.0.1:80)
> Obtain VAPID keys at https://web-push-codelab.glitch.me/
> Set VAPID keys in appsettings.json, like,

"Vapid": {
"Subject": "mailto:my@example.com",
"PublicKey": "<PUBLIC_KEY>",
"PrivateKey": "<PRIVATE_KEY>"
}

# install @angular/pwa to existing angular app

ng add @angular/pwa --project _project-name_

# To demo push notification, first we need to make a production build

ng build --prod

# install http-server if not installed

npx http-server ./dist/my-ng-app -o
