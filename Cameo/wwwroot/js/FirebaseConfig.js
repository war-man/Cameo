// Your web app's Firebase configuration
var firebaseConfig = {
    apiKey: "AIzaSyAw5KbidCabOlB590MEXVL7ESUfVNd5v7g",
    authDomain: "cameo-uz.firebaseapp.com",
    databaseURL: "https://cameo-uz.firebaseio.com",
    projectId: "cameo-uz",
    storageBucket: "cameo-uz.appspot.com",
    messagingSenderId: "518896570546",
    appId: "1:518896570546:web:33ebce552cf089ab2ef9ab",
    measurementId: "G-KDNNY83VG4"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);
firebase.analytics();

//firebase.auth().onAuthStateChanged(function (user)
//{
//    if (user)
//    {
//        console.log(user);
//        //// User is signed in.
//        //var displayName = user.displayName;
//        //var email = user.email;
//        //var emailVerified = user.emailVerified;
//        //var photoURL = user.photoURL;
//        //var isAnonymous = user.isAnonymous;
//        //var uid = user.uid;
//        //var providerData = user.providerData;
//        //// ...
//    } else
//    {
//        console.log("user is not authenticated");
//        // User is signed out.
//        // ...
//    }
//});

//// Retrieve Firebase Messaging object.
//const messaging = firebase.messaging();
//// Handle incoming messages. Called when:
//// - a message is received while the app has focus
//// - the user clicks on an app notification created by a service worker
////   `messaging.setBackgroundMessageHandler` handler.
//messaging.onMessage((payload) =>
//{
//    console.log('Message received. ', payload);
//    // ...
//});
//messaging.setBackgroundMessageHandler(function (payload)
//{
//    console.log('[firebase-messaging-sw.js] Received background message ', payload);
//    // Customize notification here
//    const notificationTitle = 'Background Message Title';
//    const notificationOptions = {
//        body: 'Background Message body.',
//        icon: '/firebase-logo.png'
//    };

//    return self.registration.showNotification(notificationTitle,
//        notificationOptions);
//});

//// Retrieve Firebase Messaging object.
//const messaging = firebase.messaging();
//// Add the public key generated from the console here.
//messaging.usePublicVapidKey("BJFQYWiV-dyU24i1CPTDcWdbfUdAC8eey-DL0X5onJAAnWcVi_J8cTAwNhqAM9dSk_TdAAJcGDp9qRemmC_OVFs");
//// Get Instance ID token. Initially this makes a network call, once retrieved
//// subsequent calls to getToken will return from cache.
//messaging.getToken().then((currentToken) =>
//{
//    if (currentToken)
//    {
//        sendTokenToServer(currentToken);
//        updateUIForPushEnabled(currentToken);
//    } else
//    {
//        // Show permission request.
//        console.log('No Instance ID token available. Request permission to generate one.');
//        // Show permission UI.
//        updateUIForPushPermissionRequired();
//        setTokenSentToServer(false);
//    }
//}).catch((err) =>
//{
//    console.log('An error occurred while retrieving token. ', err);
//    showToken('Error retrieving Instance ID token. ', err);
//    setTokenSentToServer(false);
//    });

//// Callback fired if Instance ID token is updated.
//messaging.onTokenRefresh(() =>
//{
//    messaging.getToken().then((refreshedToken) =>
//    {
//        console.log('Token refreshed.');
//        // Indicate that the new Instance ID token has not yet been sent to the
//        // app server.
//        setTokenSentToServer(false);
//        // Send Instance ID token to app server.
//        sendTokenToServer(refreshedToken);
//        // ...
//    }).catch((err) =>
//    {
//        console.log('Unable to retrieve refreshed token ', err);
//        showToken('Unable to retrieve refreshed token ', err);
//    });
//});