import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { participantsApi } from "../services/api";

function ParticipantsPage() {
  const [participants, setParticipants] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  async function loadParticipants() {
    try {
      setLoading(true);
      setError("");
      const data = await participantsApi.getAll();
      setParticipants(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadParticipants();
  }, []);

  async function handleDelete(id) {
    try {
      setError("");
      await participantsApi.delete(id);
      await loadParticipants();
    } catch (err) {
      setError(err.message);
    }
  }

  return (
    <section className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Management</p>
          <h2>Participants</h2>
        </div>
      </div>

      {error ? <div className="alert error">{error}</div> : null}

      <div className="page-grid participant-layout">
        <div className="card detail-grid">
          <h3>Admin Directory</h3>
          <p>User self-registration now flows through the public register page.</p>
          <p>This screen is reserved for admin review, edits, and deletions.</p>
        </div>
        <div className="card">
          <div className="card-header">
            <h3>Participant List</h3>
          </div>

          {loading ? <p>Loading participants...</p> : null}

          {!loading && participants.length === 0 ? <p>No participants found.</p> : null}

          <div className="stack-list">
            {participants.map((item) => (
              <article key={item.id} className="list-card">
                <div>
                  <h4>{item.fullName}</h4>
                  <p>{item.email}</p>
                  <p>{item.phoneNumber}</p>
                  <p>Role: {["Regular", "Organizer", "Admin"][item.role] ?? item.role}</p>
                </div>
                <div className="button-stack">
                  <Link className="secondary-button link-button" to={`/participants/${item.id}/edit`}>
                    Edit
                  </Link>
                  <button className="danger-button" onClick={() => handleDelete(item.id)}>
                    Delete
                  </button>
                </div>
              </article>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}

export default ParticipantsPage;
