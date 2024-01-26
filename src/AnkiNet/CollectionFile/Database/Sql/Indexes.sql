CREATE INDEX ix_cards_nid   ON cards  (nid);
CREATE INDEX ix_cards_sched ON cards  (did, queue, due);
CREATE INDEX ix_cards_usn   ON cards  (usn);
CREATE INDEX ix_notes_csum  ON notes  (csum);
CREATE INDEX ix_notes_usn   ON notes  (usn);
CREATE INDEX ix_revlog_cid  ON revlog (cid);
CREATE INDEX ix_revlog_usn  ON revlog (usn);