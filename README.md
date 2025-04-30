<p align="center">
	<img src="client/src/assets/logoLibro.svg" alt="Libro Logo" width="300"/>
</p>
<h1 align="center">Libro – Online Bookstore Web Application</h1>

<p>
  <strong>Libro</strong> is a full-stack web application modeled after the Ukrainian book store <a href="https://www.yakaboo.ua/">Yakaboo</a>.
  It enables users to browse, search, and purchase books online — including physical books, e-books, and audiobooks.
</p>

<p>
  This project was developed as a diploma thesis by a team of 5 programmers and 3 designers at <strong>IT Step Academy (Lviv)</strong>.
</p>

<h2>🧱 Architecture Overview</h2>

<ul>
  <li><strong>Backend:</strong> ASP.NET Core 8 with a microservices-based architecture</li>
  <li><strong>Frontend:</strong> React with TypeScript</li>
  <li><strong>Gateway:</strong> Ocelot API Gateway and GraphQL-based APIComposer (HotChocolate)</li>
</ul>

<hr/>

<h2>🔙 Backend Details</h2>

<h3>Microservices</h3>

<ul>
  <li>
    <strong>UserAPI</strong> – Manages users and subscriptions. Features:
    <ul>
      <li>Role-based authentication (JWT)</li>
      <li>OAuth (Google login)</li>
      <li>Roles:
        <ul>
          <li><strong>Admin</strong> – Full access to admin dashboard</li>
          <li><strong>Moderator</strong> – Partial dashboard access (no user management)</li>
          <li><strong>User</strong> – Standard access to personal features like basket, orders, and favorites</li>
        </ul>
      </li>
    </ul>
  </li>

  <li>
    <strong>BookAPI</strong> – Handles:
    <ul>
      <li>Books, authors, publishers, categories, subcategories</li>
      <li>Discounts and feedback (no edit/delete)</li>
      <li>Three book types: Paper (delivery), E-book, Audiobook (added to personal library)</li>
      <li><em>No real transactions yet – mock purchase flow only</em></li>
    </ul>
  </li>

  <li>
    <strong>OrderAPI</strong> – Processes orders manually
    <ul>
      <li>Order status managed manually by Admin/Moderator</li>
      <li>No delivery tracking implemented yet</li>
    </ul>
  </li>
</ul>

<h3>Common Backend Features</h3>

<ul>
  <li>Pagination, filtering, sorting, and searching across all major entities (books, users, orders, etc.)</li>
  <li>Redis caching for optimized GET requests</li>
  <li>Asynchronous programming used wherever possible</li>
  <li>File storage on AWS S3: user avatars, subscription images, book covers, e-book & audio-book files</li>
  <li>Access to e-books and audiobooks is secured using signed URLs</li>
  <li>Serilog for logging and monitoring</li>
  <li>Custom Exception Middleware for error handling</li>
  <li>DTOs and AutoMapper used for data transfer</li>
  <li>Database seeding for test/demo purposes</li>
</ul>

<h3>Architecture Pattern</h3>
<p><code>Microservice → Controller → Service → Repository</code></p>

<hr/>

<h2>🖥️ Frontend Details</h2>

<h3>Architecture</h3>
<p><code>apiConfig → repository → service → container → component</code></p>

<h3>Main Features</h3>

<ul>
  <li>React with TypeScript, Vite for bundling</li>
  <li>Tailwind CSS for styling</li>
  <li>Zod schema validation for form inputs</li>
  <li>Image + entity data uploads via FormData</li>
  <li>Axios for HTTP communication</li>
  <li>Authentication and user info stored in <code>localStorage</code></li>
  <li>Notifications using React/Redux</li>
  <li>Lazy loading of images</li>
  <li>Memoization and virtualization for performance</li>
  <li>Asynchronous data fetching and processing</li>
</ul>

<hr/>

<h2>🔐 Authentication & Authorization</h2>

<ul>
  <li>JWT-based role authentication</li>
  <li>OAuth (Google login)</li>
  <li>Token-protected API routes</li>
  <li>Frontend reflects role-based UI access</li>
</ul>

<hr/>

<h2>🛠️ Technologies Used</h2>

<h3>Backend</h3>
<ul>
  <li>ASP.NET Core 8</li>
  <li>EF Core</li>
  <li>Redis</li>
  <li>Ocelot Gateway</li>
  <li>GraphQL (HotChocolate)</li>
  <li>Serilog</li>
  <li>AWS S3</li>
</ul>

<h3>Frontend</h3>
<ul>
  <li>React + TypeScript</li>
  <li>Vite</li>
  <li>Redux</li>
  <li>Zod</li>
  <li>Tailwind CSS</li>
  <li>Axios</li>
</ul>

<hr/>

<h2>🧪 Development Status</h2>
<p>
  <em>Note: This project simulates the purchase flow without actual payment integrations. Features like real-time delivery tracking and payment processing are planned for future improvements.</em>
</p>

<hr/>

<h2>🤝 Credits</h2>
<p>
  Developed by a team of 5 programmers and 3 designers as part of the diploma thesis at <strong>IT Step Academy, Lviv</strong>.
</p>

<hr/>

<h2>🔗 Repository</h2>
<p>
  GitHub: <a href="https://github.com/zabavb/Libro">https://github.com/zabavb/Libro</a>
</p>
