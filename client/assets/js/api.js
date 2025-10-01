const API_BASE = ''; // mismo origen
const TOKEN_KEY = 'token';

function getToken() { return localStorage.getItem(TOKEN_KEY); }
function setToken(t) { localStorage.setItem(TOKEN_KEY, t); }
function logout() { localStorage.removeItem(TOKEN_KEY); }

async function apiFetch(path, options = {}) {
  const headers = options.headers || {};
  if (getToken()) headers['Authorization'] = 'Bearer ' + getToken();
  if (!(options.body instanceof FormData)) {
    headers['Content-Type'] = headers['Content-Type'] || 'application/json';
  }
  const res = await fetch(API_BASE + path, { ...options, headers });
  if (res.status === 401) {
    logout();
    location.href = 'index.html';
    return;
  }
  if (!res.ok) {
    const text = await res.text();
    throw new Error(text || 'Error de red');
  }
  const ct = res.headers.get('content-type') || '';
  return ct.includes('application/json') ? res.json() : res.text();
}

async function requireAuth() {
  const token = getToken();
  if (!token) { location.href = 'index.html'; return; }
  try { await apiFetch('/api/v1/auth/me'); } catch { location.href = 'index.html'; }
}
