import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";
import { eventsApi } from "../services/api";

function EventDetailPage() {
  const auth = useAuth();
  const { id } = useParams();
  const [event, setEvent] = useState(null);
  const [attendees, setAttendees] = useState([]);
  const [loading, setLoading] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [message, setMessage] = useState("");
  const [error, setError] = useState("");

  async function loadPage() {
    try {
      setLoading(true);
      setError("");

      const eventData = await eventsApi.getById(id);
      const canInspectAttendees = auth.isAuthenticated && (
        auth.isAdmin || eventData.organizerId === auth.user?.id
      );
      const participantData = canInspectAttendees
        ? await eventsApi.getParticipants(id)
        : [];

      setEvent(eventData);
      setAttendees(participantData);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadPage();
  }, [auth.isAdmin, auth.isAuthenticated, auth.isOrganizer, id]);

  async function registerParticipant() {
    if (!auth.user?.id) {
      setError("Login before registering.");
      return;
    }

    try {
      setSubmitting(true);
      setError("");
      setMessage("");
      await eventsApi.registerParticipant(id, auth.user.id);
      setMessage("Participant registered successfully.");
      await auth.refreshUser();
      await loadPage();
    } catch (err) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  }

  async function unregisterSelf() {
    try {
      setSubmitting(true);
      setError("");
      setMessage("");
      await eventsApi.unregisterParticipant(id, auth.user.id);
      setMessage("Registration removed.");
      await auth.refreshUser();
      await loadPage();
    } catch (err) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  }

  const isOwner = auth.isAuthenticated && (auth.isAdmin || event?.organizerId === auth.user?.id);
  const isRegistered = auth.isAuthenticated && Boolean(event?.participantIds?.includes(auth.user?.id));
  const canSelfRegister = auth.isAuthenticated && !isRegistered && event?.registeredParticipants < event?.maxParticipants;

  return (
    <section className="page">
      <div className="page-header">
        <div>
          <p className="eyebrow">Event</p>
          <h2>Details</h2>
        </div>
        <div className="button-row">
          <Link className="secondary-button link-button" to="/events">
            Back
          </Link>
          {isOwner ? (
            <Link className="primary-button link-button" to={`/events/${id}/edit`}>
              Edit Event
            </Link>
          ) : null}
        </div>
      </div>

      {error ? <div className="alert error">{error}</div> : null}
      {message ? <div className="alert success">{message}</div> : null}

      {loading ? <div className="card"><p>Loading event...</p></div> : null}

      {!loading && event ? (
        <div className="page-grid detail-layout">
          <div className="stack-list">
            <div className="card detail-grid">
              <h3>{event.title}</h3>
              <p>{event.description}</p>
              <p><strong>Category:</strong> {event.category}</p>
              <p><strong>Location:</strong> {event.location}</p>
              <p><strong>Organizer:</strong> {event.organizerName}</p>
              <p><strong>Status:</strong> {["Active", "Ongoing", "Completed", "Cancelled"][event.status] ?? event.status}</p>
              <p><strong>Schedule:</strong> {new Date(event.startDate).toLocaleString()} to {new Date(event.endDate).toLocaleString()}</p>
              <p><strong>Capacity:</strong> {event.registeredParticipants}/{event.maxParticipants}</p>
              <p><strong>Contact:</strong> {event.contactEmail} | {event.contactPhone}</p>
              <p><strong>Tags:</strong> {event.tags?.length ? event.tags.join(", ") : "None"}</p>
            </div>

            <div className="card form-grid">
              <div className="card-header">
                <h3>Registration</h3>
              </div>

              {!auth.isAuthenticated ? (
                <p>Login to register for this event.</p>
              ) : isRegistered ? (
                <>
                  <p>You are currently registered for this event.</p>
                  <button
                    type="button"
                    className="danger-button"
                    disabled={submitting}
                    onClick={unregisterSelf}
                  >
                    {submitting ? "Processing..." : "Unregister"}
                  </button>
                </>
              ) : (
                <>
                  <p>Registration is tied to your signed-in account.</p>
                  <button
                    type="button"
                    className="primary-button"
                    disabled={submitting || !canSelfRegister}
                    onClick={registerParticipant}
                  >
                    {submitting ? "Processing..." : "Register"}
                  </button>
                </>
              )}
            </div>
          </div>

          <div className="card detail-grid">
            <h3>{isOwner ? "Attendees" : "Access Notes"}</h3>
            {isOwner ? (
              <>
                {!attendees.length ? <p>No attendees registered yet.</p> : null}
                <div className="stack-list">
                  {attendees.map((participant) => (
                    <article key={participant.id} className="list-card">
                      <div>
                        <h4>{participant.fullName}</h4>
                        <p>{participant.email}</p>
                        <p>{participant.phoneNumber}</p>
                      </div>
                    </article>
                  ))}
                </div>
              </>
            ) : (
              <>
                <p>Only the event owner or an admin can inspect the attendee list.</p>
                <p>Regular users can register and unregister themselves from this page.</p>
              </>
            )}
          </div>
        </div>
      ) : null}
    </section>
  );
}

export default EventDetailPage;
