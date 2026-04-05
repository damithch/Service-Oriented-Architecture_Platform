import { useEffect, useState } from "react";
import { eventsApi, participantsApi } from "../services/api";
import { useAuth } from "../auth/AuthContext";

function DashboardPage() {
  const auth = useAuth();
  const [summary, setSummary] = useState({
    events: 0,
    participants: 0,
    organizers: 0,
    myRegistered: 0,
    myOrganized: 0
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    async function loadSummary() {
      try {
        setLoading(true);
        setError("");

        const events = await eventsApi.getAll();
        let participants = [];
        let organizers = [];

        if (auth.isAdmin) {
          [participants, organizers] = await Promise.all([
            participantsApi.getAll(),
            participantsApi.getOrganizers()
          ]);
        }

        setSummary({
          events: events.length,
          participants: participants.length,
          organizers: organizers.length,
          myRegistered: auth.user?.registeredEventIds?.length ?? 0,
          myOrganized: auth.user?.organizedEventIds?.length ?? 0
        });
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }

    loadSummary();
  }, [auth.isAdmin, auth.user]);

  return (
    <section className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Overview</p>
          <h2>Dashboard</h2>
        </div>
      </div>

      {error ? <div className="alert error">{error}</div> : null}

      <div className="stats-grid">
        <article className="stat-card">
          <span>Total Events</span>
          <strong>{loading ? "..." : summary.events}</strong>
        </article>
        {auth.isAdmin ? (
          <>
            <article className="stat-card">
              <span>Total Participants</span>
              <strong>{loading ? "..." : summary.participants}</strong>
            </article>
            <article className="stat-card">
              <span>Organizers</span>
              <strong>{loading ? "..." : summary.organizers}</strong>
            </article>
          </>
        ) : (
          <>
            <article className="stat-card">
              <span>My Registrations</span>
              <strong>{loading ? "..." : summary.myRegistered}</strong>
            </article>
            <article className="stat-card">
              <span>My Organized Events</span>
              <strong>{loading ? "..." : summary.myOrganized}</strong>
            </article>
          </>
        )}
      </div>

      <div className="card">
        <h3>Current Session</h3>
        <p>
          Signed in as <strong>{auth.user?.fullName}</strong> with role{" "}
          <strong>{["Regular", "Organizer", "Admin"][auth.user?.role] ?? auth.user?.role}</strong>.
        </p>
        <p>
          Organizers can manage their own events. Admins can manage users and all events.
        </p>
      </div>
    </section>
  );
}

export default DashboardPage;
