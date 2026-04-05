import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { eventsApi, participantsApi } from "../services/api";
import { readFileAsDataUrl } from "../utils/fileUtils";

function EventEditPage() {
  const auth = useAuth();
  const { id } = useParams();
  const navigate = useNavigate();
  const [form, setForm] = useState(null);
  const [organizers, setOrganizers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadPage() {
      try {
        setLoading(true);
        setError("");

        const eventData = await eventsApi.getById(id);
        const organizersData = auth.isAdmin ? await participantsApi.getOrganizers() : [];

        setForm({
          ...eventData,
          startDate: eventData.startDate?.slice(0, 16) ?? "",
          endDate: eventData.endDate?.slice(0, 16) ?? "",
          tags: eventData.tags?.join(", ") ?? ""
        });
        setOrganizers(organizersData);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadPage();
  }, [auth.isAdmin, id]);

  function onChange(event) {
    const { name, value } = event.target;
    setForm((current) => ({ ...current, [name]: value }));
  }

  async function onImageChange(event) {
    const file = event.target.files?.[0];
    if (!file) {
      return;
    }

    try {
      const dataUrl = await readFileAsDataUrl(file);
      setForm((current) => ({ ...current, imageUrl: dataUrl }));
    } catch (err) {
      setError(err.message);
    }
  }

  function onOrganizerChange(event) {
    const organizerId = event.target.value;
    const organizer = organizers.find((item) => item.id === organizerId);

    setForm((current) => ({
      ...current,
      organizerId,
      organizerName: organizer?.fullName ?? current.organizerName
    }));
  }

  async function onSubmit(event) {
    event.preventDefault();

    try {
      setSaving(true);
      setError("");

      await eventsApi.update(id, {
        ...form,
        maxParticipants: Number(form.maxParticipants),
        registeredParticipants: Number(form.registeredParticipants),
        status: Number(form.status),
        tags: form.tags.split(",").map((tag) => tag.trim()).filter(Boolean)
      });

      navigate(`/events/${id}`);
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  return (
    <section className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Event</p>
          <h2>Edit</h2>
        </div>
        <Link className="secondary-button link-button" to={`/events/${id}`}>
          Back to Event
        </Link>
      </div>

      {error ? <div className="alert error">{error}</div> : null}

      {loading ? <div className="card"><p>Loading event...</p></div> : null}

      {!loading && form ? (
        <form className="card form-grid" onSubmit={onSubmit}>
          <label>
            <span>Title</span>
            <input name="title" value={form.title} onChange={onChange} required />
          </label>

          <label>
            <span>Description</span>
            <textarea name="description" value={form.description} onChange={onChange} rows="4" required />
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
            <div className="inline-note">Organizer ownership is locked to your account.</div>
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
            <input type="datetime-local" name="startDate" value={form.startDate} onChange={onChange} required />
          </label>

          <label>
            <span>End Date</span>
            <input type="datetime-local" name="endDate" value={form.endDate} onChange={onChange} required />
          </label>

          <label>
            <span>Max Participants</span>
            <input type="number" min="1" name="maxParticipants" value={form.maxParticipants} onChange={onChange} required />
          </label>

          <label>
            <span>Registered Participants</span>
            <input type="number" min="0" name="registeredParticipants" value={form.registeredParticipants} onChange={onChange} required />
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
            <input type="email" name="contactEmail" value={form.contactEmail} onChange={onChange} required />
          </label>

          <label>
            <span>Contact Phone</span>
            <input name="contactPhone" value={form.contactPhone} onChange={onChange} required />
          </label>

          <label>
            <span>Event Image</span>
            <input type="file" accept="image/*" onChange={onImageChange} />
          </label>

          {form.imageUrl ? (
            <div className="image-preview-card">
              <img className="event-image-preview" src={form.imageUrl} alt={`${form.title} preview`} />
            </div>
          ) : null}

          <label>
            <span>Tags</span>
            <input name="tags" value={form.tags} onChange={onChange} />
          </label>

          <button type="submit" className="primary-button" disabled={saving}>
            {saving ? "Saving..." : "Update Event"}
          </button>
        </form>
      ) : null}
    </section>
  );
}

export default EventEditPage;
