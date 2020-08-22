'use strict';

var cacheVersion = 2;
var currentCache = {
    offline: 'bufunfa-cache' + cacheVersion
};

this.addEventListener('install', event => {
    event.waitUntil(
        caches.open(currentCache.offline).then(function (cache) {
            return cache.addAll([
                '/plugins/plugins.bundle.css',
                '/plugins/plugins.bundle.js',
                '/css/style.bundle.css',
                '/js/scripts.bundle.js',
                '/vendors/jquery-confirm/jquery-confirm.min.css',
                '/vendors/select2/i18n/pt-BR.js',
                '/vendors/bootstrap-datepicker/bootstrap-datepicker.pt-BR.min.js',
                '/vendors/moment/pt-br.js',
                '/vendors/jquery-confirm/jquery-confirm.min.js',
                '/vendors/numerals/numeral.min.js',
                '/vendors/countUp/countup.min.js',
                '/vendors/snap-svg/snap.svg.min.js',
                '/vendors/liquid-meter/liquid.meter.min.js',
                '/vendors/offline/offline.min.js',
                '/vendors/offline/offline-js.min.css',
                '/img/loading.svg'
            ]);
        })
    );
});

this.addEventListener('fetch', function (event) {
    event.respondWith(
        caches.match(event.request).then(function (response) {
            return response || fetch(event.request);
        })
    );
});

this.addEventListener("beforeinstallprompt", function (e) {
    e.userChoice.then(function (choiceResult) {

        console.log(choiceResult.outcome);

        if (choiceResult.outcome == "dismissed") {
            console.log("User cancelled home screen install");
        }
        else {
            console.log("User added to home screen");
        }
    });
});