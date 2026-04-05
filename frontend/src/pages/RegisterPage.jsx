import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { authApi } from "../services/api";

const initialForm = {
  fullName: "",
  email: "",
  password: "",
  phoneNumber: "",
  address: "",
  role: 0
};

function RegisterPage() {
  const auth = useAuth();
  const navigate = useNavigate();
  const [form, setForm] = useState(initialForm);
  const [error, setError] = useState("");
  const [saving, setSaving] = useState(false);

  function onChange(event) {
    const { name, value } = event.target;
    setForm((current) => ({ ...current, [name]: value }));
  }

  async function onSubmit(event) {
    event.preventDefault();

    try {
      setSaving(true);
      setError("");
      const response = await authApi.register({
        ...form,
        role: Number(form.role)
      });
      auth.saveSession(response);
      navigate("/", { replace: true });
    } catch (err) {
      setError(err.message);
    } finally {
      setSaving(false);
    }
  }

  return (
    <section className="page auth-page">
      <form className="card form-grid auth-card" onSubmit={onSubmit}>
        <div className="card-header">
          <p className="eyebrow">Access</p>
          <h2>Register</h2>
        </div>

        {error ? <div className="alert error">{error}</div> : null}

        <label>
          <span>Full Name</span>
          <input name="fullName" value={form.fullName} onChange={onChange} required />
        </label>

        <label>
          <span>Email</span>
          <input type="email" name="email" value={form.email} onChange={onChange} required />
        </label>

        <label>
          <span>Password</span>
          <input type="password" name="password" value={form.password} onChange={onChange} minLength="6" required />
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
          <span>Account Type</span>
          <select name="role" value={form.role} onChange={onChange}>
            <option value="0">Regular</option>
            <option value="1">Organizer</option>
          </select>
        </label>

        <button type="submit" className="primary-button" disabled={saving}>
          {saving ? "Creating..." : "Create Account"}
        </button>

        <p className="muted-text">
          Already registered? <Link to="/login">Login here</Link>
        </p>
      </form>
    </section>
  );
}

export default RegisterPage;
