import { API_BASE_URL } from "../config";
import { getStoredToken } from "../auth/storage";

async function request(path, options = {}) {
  const token = getStoredToken();
  const response = await fetch(`${API_BASE_URL}${path}`, {
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
      ...(options.headers ?? {})
    },
    ...options
  });

  if (!response.ok) {
    let message = "Request failed";

    try {
      const errorBody = await response.json();
      message = errorBody.message ?? JSON.stringify(errorBody);
    } catch {
      message = response.statusText || message;
    }

    throw new Error(message);
  }

  if (response.status === 204) {
    return null;
  }

  return response.json();
}

export const eventsApi = {
  getAll() {
    return request("/events");
  },
  getById(id) {
    return request(`/events/${id}`);
  },
  getParticipants(id) {
    return request(`/events/${id}/participants`);
  },
  searchByTitle(title) {
    return request(`/events/search/title/${encodeURIComponent(title)}`);
  },
  searchByCategory(category) {
    return request(`/events/search/category/${encodeURIComponent(category)}`);
  },
  searchByDateRange(startDate, endDate) {
    const query = new URLSearchParams({ startDate, endDate }).toString();
    return request(`/events/search/daterange?${query}`);
  },
  create(payload) {
    return request("/events", {
      method: "POST",
      body: JSON.stringify(payload)
    });
  },
  update(id, payload) {
    return request(`/events/${id}`, {
      method: "PUT",
      body: JSON.stringify(payload)
    });
  },
  delete(id) {
    return request(`/events/${id}`, {
      method: "DELETE"
    });
  },
  registerParticipant(eventId, participantId) {
    return request(`/events/${eventId}/register/${participantId}`, {
      method: "POST"
    });
  },
  unregisterParticipant(eventId, participantId) {
    return request(`/events/${eventId}/unregister/${participantId}`, {
      method: "DELETE"
    });
  }
};

export const participantsApi = {
  getAll() {
    return request("/participants");
  },
  getById(id) {
    return request(`/participants/${id}`);
  },
  getOrganizers() {
    return request("/participants/organizers/list");
  },
  create(payload) {
    return request("/participants", {
      method: "POST",
      body: JSON.stringify(payload)
    });
  },
  update(id, payload) {
    return request(`/participants/${id}`, {
      method: "PUT",
      body: JSON.stringify(payload)
    });
  },
  delete(id) {
    return request(`/participants/${id}`, {
      method: "DELETE"
    });
  }
};

export const authApi = {
  register(payload) {
    return request("/auth/register", {
      method: "POST",
      body: JSON.stringify(payload)
    });
  },
  login(payload) {
    return request("/auth/login", {
      method: "POST",
      body: JSON.stringify(payload)
    });
  },
  me() {
    return request("/auth/me");
  }
};
