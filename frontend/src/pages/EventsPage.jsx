import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { participantsApi, eventsApi } from "../services/api";

const initialForm = {
  title: "",
  description: "",
  organizerId: "",
  organizerName: "",
  startDate: "",
  endDate: "",
  location: "",
  category: "",
  maxParticipants: 0,
  registeredParticipants: 0,
  status: 0,
  contactEmail: "",
  contactPhone: "",
  tags: ""
};

const initialSearch = {
  mode: "all",
  title: "",
  category: "",
  startDate: "",
  endDate: ""
};

function EventsPage() {
  const auth = useAuth();
  const [events, setEvents] = useState([]);
  const [organizers, setOrganizers] = useState([]);
  const [form, setForm] = useState(initialForm);
  const [search, setSearch] = useState(initialSearch);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  async function loadData() {
    try {
      setLoading(true);
      setError("");

      const eventsData = await eventsApi.getAll();
      const organizersData = auth.isAuthenticated && (auth.isOrganizer || auth.isAdmin)
        ? await participantsApi.getOrganizers()
        : [];

      setEvents(eventsData);
      setOrganizers(organizersData);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadData();
  }, [auth.isAdmin, auth.isAuthenticated, auth.isOrganizer]);

  function onChange(event) {
    const { name, value } = event.target;
    setForm((current) => ({ ...current, [name]: value }));
  }

  function onSearchChange(event) {
    const { name, value } = event.target;
    setSearch((current) => ({ ...current, [name]: value }));
  }

  function onOrganizerChange(event) {
    const organizerId = event.target.value;
    const organizer = organizers.find((item) => item.id === organizerId);

    setForm((current) => ({
      ...current,
      organizerId,
      organizerName: organizer?.fullName ?? ""
    }));
  }

  async function onSubmit(event) {
    event.preventDefault();

    try {
      setSaving(true);
      setError("");

      const organizerId = auth.isAdmin ? form.organizerId : auth.user.id;
      const organizerName = auth.isAdmin
        ? organizers.find((item) => item.id === form.organizerId)?.fullName ?? form.organizerName
        : auth.user.fullName;

      await eventsApi.create({
        ...form,
        organizerId,
        organizerName,
        maxParticipants: Number(form.maxParticipants),
        registeredParticipants: Number(form.registeredParticipants),
        status: Number(form.status),
        tags: form.tags.split(",").map((tag) => tag.trim()).filter(Boolean)
      });

      setForm(initialForm);
      await loadData();
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(id) {
    try {
      setError("");
      await eventsApi.delete(id);
      await loadData();
    } catch (err) {
      setError(err.message);
    }
  }

  async function handleSearch(event) {
    event.preventDefault();

    try {
      setLoading(true);
      setError("");

      let results = [];

      if (search.mode === "title") {
        results = await eventsApi.searchByTitle(search.title);
      } else if (search.mode === "category") {
        results = await eventsApi.searchByCategory(search.category);
      } else if (search.mode === "daterange") {
        results = await eventsApi.searchByDateRange(search.startDate, search.endDate);
      } else {
        results = await eventsApi.getAll();
      }

      setEvents(results);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  async function resetSearch() {
    setSearch(initialSearch);
    await loadData();
  }

  return (
    <section className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Management</p>
          <h2>Events</h2>
        </div>
      </div>

      {error ? <div className="alert error">{error}</div> : null}

      <div className="page-grid">
        <div className="stack-list">
          <form className="card form-grid" onSubmit={handleSearch}>
            <div className="card-header">
              <h3>Search Events</h3>
            </div>

            <label>
              <span>Search Type</span>
              <select name="mode" value={search.mode} onChange={onSearchChange}>
                <option value="all">All events</option>
                <option value="title">By title</option>
                <option value="category">By category</option>
                <option value="daterange">By date range</option>
              </select>
            </label>

            {search.mode === "title" ? (
              <label>
                <span>Title</span>
                <input name="title" value={search.title} onChange={onSearchChange} required />
              </label>
            ) : null}

            {search.mode === "category" ? (
              <label>
                <span>Category</span>
                <input name="category" value={search.category} onChange={onSearchChange} required />
              </label>
            ) : null}

            {search.mode === "daterange" ? (
              <>
                <label>
                  <span>Start Date</span>
                  <input
                    type="datetime-local"
                    name="startDate"
                    value={search.startDate}
                    onChange={onSearchChange}
                    required
                  />
                </label>
                <label>
                  <span>End Date</span>
                  <input
                    type="datetime-local"
                    name="endDate"
                    value={search.endDate}
                    onChange={onSearchChange}
                    required
                  />
                </label>
              </>
            ) : null}

            <div className="button-row">
              <button type="submit" className="primary-button">
                Search
              </button>
              <button type="button" className="secondary-button" onClick={resetSearch}>
                Reset
              </button>
            </div>
          </form>

          {auth.isAuthenticated && (auth.isOrganizer || auth.isAdmin) ? (
            <form className="card form-grid" onSubmit={onSubmit}>
              <div className="card-header">
                <h3>Create Event</h3>
              </div>

              <label>
                <span>Title</span>
                <input name="title" value={form.title} onChange={onChange} required />
              </label>

              <label>
                <span>Description</span>
                <textarea
                  name="description"
                  value={form.description}
                  onChange={onChange}
                  rows="4"
                  required
                />
              </label>

              {auth.isAdmin ? (
                <label>
                  <span>Organizer</span>
                  <select name="organizerId" value={form.organizerId} onChange={onOrganizerChange} required>
                    <option value="">Select organizer</option>
                    {organizers.map((organizer) => (
                      <option key={organizer.id} value={organizer.id}>
                        {organizer.fullName}
                      </option>
                    ))}
                  </select>
                </label>
              ) : (
                <div className="inline-note">This event will be created under your organizer account.</div>
              )}

              <label>
                <span>Category</span>
                <input name="category" value={form.category} onChange={onChange} required />
              </label>

              <label>
                <span>Location</span>
                <input name="location" value={form.location} onChange={onChange} required />
              </label>

              <label>
                <span>Start Date</span>
                <input
                  type="datetime-local"
                  name="startDate"
                  value={form.startDate}
                  onChange={onChange}
                  required
                />
              </label>

              <label>
                <span>End Date</span>
                <input
                  type="datetime-local"
                  name="endDate"
                  value={form.endDate}
                  onChange={onChange}
                  required
                />
              </label>

              <label>
                <span>Max Participants</span>
                <input
                  type="number"
                  min="1"
                  name="maxParticipants"
                  value={form.maxParticipants}
                  onChange={onChange}
                  required
                />
              </label>

              <label>
                <span>Status</span>
                <select name="status" value={form.status} onChange={onChange}>
                  <option value="0">Active</option>
                  <option value="1">Ongoing</option>
                  <option value="2">Completed</option>
                  <option value="3">Cancelled</option>
                </select>
              </label>

              <label>
                <span>Contact Email</span>
                <input
                  type="email"
                  name="contactEmail"
                  value={form.contactEmail}
                  onChange={onChange}
                  required
                />
              </label>

              <label>
                <span>Contact Phone</span>
                <input name="contactPhone" value={form.contactPhone} onChange={onChange} required />
              </label>

              <label>
                <span>Tags</span>
                <input
                  name="tags"
                  value={form.tags}
                  onChange={onChange}
                  placeholder="music, public, outdoor"
                />
              </label>

              <button type="submit" className="primary-button" disabled={saving}>
                {saving ? "Saving..." : "Create Event"}
              </button>
            </form>
          ) : (
            <div className="card detail-grid">
              <h3>Organizer Access</h3>
              <p>Sign in as an organizer or admin to create and manage events.</p>
            </div>
          )}
        </div>

        <div className="card">
          <div className="card-header">
            <h3>Event List</h3>
          </div>

          {loading ? <p>Loading events...</p> : null}

          {!loading && events.length === 0 ? <p>No events found.</p> : null}

          <div className="stack-list">
            {events.map((item) => (
              <article key={item.id} className="list-card">
                <div>
                  <h4>{item.title}</h4>
                  <p>{item.category} | {item.location}</p>
                  <p>
                    {new Date(item.startDate).toLocaleString()} to {" "}
                    {new Date(item.endDate).toLocaleString()}
                  </p>
                  <p>
                    Organizer: {item.organizerName} | Registered: {item.registeredParticipants}/
                    {item.maxParticipants}
                  </p>
                </div>
                <div className="button-stack">
                  <Link className="secondary-button link-button" to={`/events/${item.id}`}>
                    View
                  </Link>
                  {auth.isAuthenticated && (auth.isAdmin || item.organizerId === auth.user?.id) ? (
                    <>
                      <Link className="secondary-button link-button" to={`/events/${item.id}/edit`}>
                        Edit
                      </Link>
                      <button className="danger-button" onClick={() => handleDelete(item.id)}>
                        Delete
                      </button>
                    </>
                  ) : null}
                </div>
              </article>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}

export default EventsPage;
