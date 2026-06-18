// Service Worker — Production mode
// This file is transformed by the Blazor build pipeline to include the list of
// static assets (self.assetsManifest). It implements a cache-first strategy
// so the app works fully offline after the first load.

// Cache version — increment to force cache refresh on update
const cacheVersion = 'v1';
const cacheName = `order-mgmt-${cacheVersion}`;

// Files to always fetch from network (dynamic or user data)
const offlineFallback = 'offline.html';

self.addEventListener('install', event => {
    // Pre-cache all assets listed in the Blazor assets manifest
    event.waitUntil(onInstall(event));
});

self.addEventListener('activate', event => {
    // Remove old caches
    event.waitUntil(onActivate(event));
});

self.addEventListener('fetch', event => {
    // Handle fetch with cache-first strategy
    event.respondWith(onFetch(event));
});

async function onInstall(event) {
    self.skipWaiting();

    const assetsRequests = self.assetsManifest?.assets
        ?.filter(a => a.url)
        ?.map(a => new Request(a.url, { integrity: a.hash, cache: 'no-cache' })) ?? [];

    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onActivate(event) {
    // Delete all caches that don't match the current version
    const keys = await caches.keys();
    for (const key of keys) {
        if (key !== cacheName) {
            await caches.delete(key);
        }
    }
    await self.clients.claim();
}

async function onFetch(event) {
    // Only handle GET requests
    if (event.request.method !== 'GET') return fetch(event.request);

    // Try cache first, then network
    const cachedResponse = await caches.match(event.request);
    if (cachedResponse) return cachedResponse;

    try {
        const networkResponse = await fetch(event.request);
        // Cache successful responses for future offline use
        if (networkResponse.ok) {
            const cache = await caches.open(cacheName);
            cache.put(event.request, networkResponse.clone());
        }
        return networkResponse;
    } catch {
        // If both cache and network fail, return a minimal offline response
        return new Response('অফলাইন মোড: ইন্টারনেট সংযোগ নেই।', {
            status: 503,
            headers: { 'Content-Type': 'text/plain; charset=utf-8' }
        });
    }
}
