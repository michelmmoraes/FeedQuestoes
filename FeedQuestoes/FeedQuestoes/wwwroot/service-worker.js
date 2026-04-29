// service-worker.js - Motor de Vanguarda
self.addEventListener('install', event => {
    console.log('[Motor] Instalado com sucesso.');
    self.skipWaiting(); // Força o motor a assumir o controle imediatamente
});

self.addEventListener('activate', event => {
    console.log('[Motor] Ativado e operante.');
    event.waitUntil(clients.claim());
});

self.addEventListener('fetch', event => {
    // Por enquanto, deixa o fluxo da rede livre (sem cache forçado)
    // Implementaremos a soberania offline plena no próximo passo.
    return;
});