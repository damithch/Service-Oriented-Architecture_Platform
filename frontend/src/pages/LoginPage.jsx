import { useState } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { authApi } from "../services/api";

function LoginPage() {
  const auth = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [form, setForm] = useState({ email: "", password: "" });
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
      const response = await authApi.login(form);
      auth.saveSession(response);
      navigate(location.state?.from ?? "/", { replace: true });
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
          <h2>Login</h2>
        </div>

        {error ? <div className="alert error">{error}</div> : null}

        <label>
          <span>Email</span>
          <input type="email" name="email" value={form.email} onChange={onChange} required />
        </label>

        <label>
          <span>Password</span>
          <input type="password" name="password" value={form.password} onChange={onChange} required />
        </label>

        <button type="submit" className="primary-button" disabled={saving}>
          {saving ? "Signing in..." : "Login"}
        </button>

        <p className="muted-text">
          Need an account? <Link to="/register">Register here</Link>
        </p>
      </form>
    </section>
  );
}

export default LoginPage;
