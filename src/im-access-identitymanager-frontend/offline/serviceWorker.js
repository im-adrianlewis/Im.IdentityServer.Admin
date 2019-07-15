const SW_VERSION = 'V1';
const CACHE_NAME = `simple-cache-${SW_VERSION}`;
const expectedCaches = [`simple-cache-${SW_VERSION}`];
const urlsToCache = ["/"];

self.addEventListener("install", event => {
  const preLoaded = caches
    .open(CACHE_NAME)
    .then(cache => {
      cache.addAll(urlsToCache);
    });
  event.waitUntil(preLoaded);
});

self.addEventListener("fetch", event => {
  const cacheResponse = caches
    .match(event.request)
    .then(match => {
      // Cache hit - return response
      if (match) {
        return match;
      }
      
      // Issue network request
      return fetch(event.request)
        .then(fetchResponse => {
          // Check if we received a valid response
          if (!fetchResponse || fetchResponse.status !== 200 || fetchResponse.type !== 'basic') {
            return fetchResponse;
          }

          // IMPORTANT: Clone the response.
          // A response is a stream and because we want the browser to consume
          //  the response as well as the cache consuming the response, we need
          //  to clone it so we have two streams.
          var responseToCache = fetchResponse.clone();

          caches.open(CACHE_NAME)
            .then(cache => {
              cache.put(event.request, responseToCache);
            });

          return fetchResponse;
        });
    });
  event.respondWith(cacheResponse);
});

self.addEventListener("activate", event => {
  const cacheResponse = caches
    .keys()
    .then(keys => Promise.all(
      keys.map(key => {
        if (!expectedCaches.includes(key)) {
          return caches.delete(key);
        }
      })
    ))
    .then(() => {
      console.log(`${SW_VERSION} now ready to handle fetches!`);
    });
  event.waitUntil(cacheResponse);
});
