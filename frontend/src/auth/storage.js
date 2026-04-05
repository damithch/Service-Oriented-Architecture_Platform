const storageKey = "kmc-auth-session";

export function loadStoredSession() {
  const raw = localStorage.getItem(storageKey);
  if (!raw) {
    return { token: "", user: null };
  }

  try {
    return JSON.parse(raw);
  } catch {
    return { token: "", user: null };
  }
}

export function getStoredToken() {
  return loadStoredSession().token ?? "";
}

export function saveStoredSession(session) {
  localStorage.setItem(storageKey, JSON.stringify(session));
}

export function clearStoredSession() {
  localStorage.removeItem(storageKey);
}
