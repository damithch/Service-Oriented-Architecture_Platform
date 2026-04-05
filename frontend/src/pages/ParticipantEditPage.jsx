import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { participantsApi } from "../services/api";

function ParticipantEditPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [form, setForm] = useState(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadParticipant() {
      try {
        setLoading(true);
        setError("");
        const data = await participantsApi.getById(id);
        setForm(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadParticipant();
  }, [id]);

  function onChange(event) {
    const { name, value, type, checked } = event.target;

    setForm((current) => ({
      ...current,
      [name]: type === "checkbox" ? checked : value
    }));
  }

  async function onSubmit(event) {
    event.preventDefault();

    try {
      setSaving(true);
      setError("");
      await participantsApi.update(id, {
        ...form,
        role: Number(form.role)
      });
      navigate("/participants");
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
          <p className="eyebrow">Participant</p>
          <h2>Edit</h2>
        </div>
        <Link className="secondary-button link-button" to="/participants">
          Back
        </Link>
      </div>

      {error ? <div className="alert error">{error}</div> : null}

      {loading ? <div className="card"><p>Loading participant...</p></div> : null}

      {!loading && form ? (
        <form className="card form-grid" onSubmit={onSubmit}>
          <label>
            <span>Full Name</span>
            <input name="fullName" value={form.fullName} onChange={onChange} required />
          </label>

          <label>
            <span>Email</span>
            <input type="email" name="email" value={form.email} onChange={onChange} required />
          </label>

          <label>
            <span>Phone Number</span>
            <input name="phoneNumber" value={form.phoneNumber} onChange={onChange} required />
          </label>

          <label>
            <span>Address</span>
            <textarea name="address" value={form.address} onChange={onChange} rows="3" required />
          </label>

          <label>
            <span>Role</span>
            <select name="role" value={form.role} onChange={onChange}>
              <option value="0">Regular</option>
              <option value="1">Organizer</option>
              <option value="2">Admin</option>
            </select>
          </label>

          <label className="checkbox-row">
            <input type="checkbox" name="isActive" checked={form.isActive} onChange={onChange} />
            <span>Active account</span>
          </label>

          <button type="submit" className="primary-button" disabled={saving}>
            {saving ? "Saving..." : "Update Participant"}
          </button>
        </form>
      ) : null}
    </section>
  );
}

export default ParticipantEditPage;
