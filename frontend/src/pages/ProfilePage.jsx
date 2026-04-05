import { useEffect, useState } from "react";
import { useAuth } from "../auth/AuthContext";
import { participantsApi } from "../services/api";

function ProfilePage() {
  const auth = useAuth();
  const [form, setForm] = useState(null);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");

  useEffect(() => {
    if (auth.user) {
      setForm({
        ...auth.user,
        phoneNumber: auth.user.phoneNumber ?? "",
        address: auth.user.address ?? ""
      });
    }
  }, [auth.user]);

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
      setMessage("");
      await participantsApi.update(auth.user.id, {
        ...form,
        role: Number(form.role)
      });
      await auth.refreshUser();
      setMessage("Profile updated.");
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  if (!form) {
    return <div className="card"><p>Loading profile...</p></div>;
  }

  return (
    <section className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Account</p>
          <h2>Profile</h2>
        </div>
      </div>

      {error ? <div className="alert error">{error}</div> : null}
      {message ? <div className="alert success">{message}</div> : null}

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

        <label className="checkbox-row">
          <input type="checkbox" name="isActive" checked={form.isActive} onChange={onChange} />
          <span>Active account</span>
        </label>

        <button type="submit" className="primary-button" disabled={saving}>
          {saving ? "Saving..." : "Update Profile"}
        </button>
      </form>
    </section>
  );
}

export default ProfilePage;
