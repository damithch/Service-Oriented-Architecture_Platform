import { createContext, useContext, useEffect, useState } from "react";
import { authApi } from "../services/api";
import {
  clearStoredSession,
  loadStoredSession,
  saveStoredSession
} from "./storage";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [session, setSession] = useState(() => loadStoredSession());
  const [ready, setReady] = useState(false);

  useEffect(() => {
    async function hydrate() {
      const currentToken = loadStoredSession().token ?? "";
      if (!currentToken) {
        setReady(true);
        return;
      }

      try {
        const user = await authApi.me();
        const nextSession = { token: currentToken, user };
        saveStoredSession(nextSession);
        setSession(nextSession);
      } catch {
        clearStoredSession();
        setSession({ token: "", user: null });
      } finally {
        setReady(true);
      }
    }

    hydrate();
  }, []);

  function saveSession(authResponse) {
    const nextSession = { token: authResponse.token, user: authResponse.user };
    saveStoredSession(nextSession);
    setSession(nextSession);
  }

  function clearSession() {
    clearStoredSession();
    setSession({ token: "", user: null });
  }

  const value = {
    token: session.token,
    user: session.user,
    ready,
    isAuthenticated: Boolean(session.token && session.user),
    isAdmin: session.user?.role === 2,
    isOrganizer: session.user?.role === 1,
    saveSession,
    clearSession,
    refreshUser: async () => {
      const user = await authApi.me();
      const nextSession = { token: loadStoredSession().token ?? "", user };
      saveStoredSession(nextSession);
      setSession(nextSession);
      return user;
    }
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }

  return context;
}
