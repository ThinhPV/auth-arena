# Auth-Arena

**Warning:** This is a *playground* — kiddie code, full of bugs, written purely for **messing around**.  

Auth-Arena is a dummy project for:
- Experimenting with **ACL** (Access Control Lists) — “who’s allowed to do what”.
- Playing with **RBAC** (Role-Based Access Control) — assigning roles (admin, user, bovinator).
- Protecting APIs with multiple schemes: **JWT**, **cookie-based ACL**, weird **PolicySchemes**, and more.
- Running silly experiments like “shared session between authenticated and anonymous users”, mock token issuers, and simulating everything until it breaks.

## Who is this for?
- Devs who love breaking things to understand them better.
- QA folks who want to test bizarre edge cases.
- Newbies in authentication who need a safe place to make mistakes.

## Features (so far)
- Demo API with endpoints: JWT-only, ACL-cookie-only, and hybrid.
- Sample `AclCookieAuthenticationHandler`.
- `IClaimsTransformation` to extend claims from cookies.
- Policy-based authorization examples for awkward scenarios.

> **Serious(ish) note:** Don’t use this code directly in production.  
> If something works *too well*, it’s either luck or a bug. Audit first.

---

⚠️ **Disclaimer:** If you blindly copy-paste this into production, side effects may include: disappearing permissions, sessions shared with cows 🐄, or your security team forcing you to rewrite it all from scratch. You have been warned.
