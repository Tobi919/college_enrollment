document.addEventListener('DOMContentLoaded', async () => {
  await requireAuth();
  const tbody = document.getElementById('tbodyStudents');
  const form = document.getElementById('formStudent');
  const btnReset = document.getElementById('btnReset');

  const qInput = document.getElementById('q');
  const searchForm = document.getElementById('searchForm');
  const prevBtn = document.getElementById('prevPage');
  const nextBtn = document.getElementById('nextPage');
  const pageInfo = document.getElementById('pageInfo');
  let page = 1;
  const pageSize = 10;
  let total = 0;

  async function load() {
    const q = encodeURIComponent(qInput.value || '');
    const res = await apiFetch(`/api/v1/students?q=${q}&page=${page}&pageSize=${pageSize}`);
    total = res.total;
    const items = res.items;
    tbody.innerHTML = '';
    for (const s of items) {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${s.Id}</td>
        <td>${s.Nombre}</td>
        <td>${s.Apellido}</td>
        <td>${s.Email}</td>
        <td>${s.FechaNacimiento || ''}</td>
		<td>${s.NumeroTelefono || ''}</td>
        <td>
          <button data-edit="${s.Id}">Editar</button>
          <button data-del="${s.Id}">Borrar</button>
        </td>`;
      tbody.appendChild(tr);
    }
    const totalPages = Math.max(1, Math.ceil(total / pageSize));
    pageInfo.textContent = `Página ${page} de ${totalPages} — ${total} registros`;
    prevBtn.disabled = page <= 1;
    nextBtn.disabled = page >= totalPages;
  }

  tbody.addEventListener('click', async (e) => {
    const idEdit = e.target.getAttribute('data-edit');
    const idDel = e.target.getAttribute('data-del');
    if (idEdit) {
      const s = await apiFetch('/api/v1/students/' + idEdit);
      document.getElementById('id').value = s.Id;
      document.getElementById('nombre').value = s.Nombre;
      document.getElementById('apellido').value = s.Apellido;
      document.getElementById('email').value = s.Email;
      document.getElementById('fecha_nacimiento').value = s.FechaNacimiento ? s.FechaNacimiento.substring(0,10) : '';
	  document.getElementById('nro_telefono').value = s.NumeroTelefono;

    }
    if (idDel) {
      if (!confirm('¿Borrar estudiante #' + idDel + '?')) return;
      await apiFetch('/api/v1/students/' + idDel, { method: 'DELETE' });
      await load();
    }
  });

  form.addEventListener('submit', async (e) => {
    e.preventDefault();
    const payload = {
      nombre: document.getElementById('nombre').value.trim(),
      apellido: document.getElementById('apellido').value.trim(),
      email: document.getElementById('email').value.trim(),
      fechaNacimiento: document.getElementById('fecha_nacimiento').value || null,
	  nroTelefono: document.getElementById('nro_telefono').value.trim()
    };
    const id = document.getElementById('id').value;
    if (id) {
      await apiFetch('/api/v1/students/' + id, { method: 'PUT', body: JSON.stringify(payload) });
    } else {
      await apiFetch('/api/v1/students', { method: 'POST', body: JSON.stringify(payload) });
    }
    form.reset();
    await load();
  });

  btnReset.addEventListener('click', () => form.reset());
  searchForm.addEventListener('submit', (e) => { e.preventDefault(); page = 1; load(); });
  prevBtn.addEventListener('click', () => { if (page > 1) { page--; load(); } });
  nextBtn.addEventListener('click', () => { page++; load(); });

  await load();
});
