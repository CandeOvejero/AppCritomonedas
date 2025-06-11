const { createApp } = Vue;

createApp({
  data() {
    return {
      vista: 'inicio',
      apiBase: "https://localhost:7126",
      transacciones: [],
      form: {
        id: null,
        cryptoCode: "",
        action: "",
        cryptoAmount: null,
        money: 0,
        dateTime: ""
      },
      editando: false,
      analisis: [],
      totalCartera: 0,
      comprobanteActual: null,
      grafico: null
    };
  },
  methods: {
    obtenerTransacciones() {
      axios.get(`${this.apiBase}/Transaccion`)
        .then(res => this.transacciones = res.data)
        .catch(err => console.error("Error al obtener transacciones", err));
    },
    guardarTransaccion() {
      const payload = { ...this.form };
      if (!this.editando) {
        delete payload.id;
        axios.post(`${this.apiBase}/Transaccion`, payload)
          .then(() => {
            this.obtenerTransacciones();
            this.resetForm();
          })
          .catch(err => {
            if (err.response?.data) {
              alert(typeof err.response.data === "string"
                ? err.response.data
                : JSON.stringify(err.response.data));
            } else {
              alert("Error inesperado al guardar.");
            }
          });
      } else {
        axios.patch(`${this.apiBase}/Transaccion/${payload.id}`, payload)
          .then(() => {
            this.obtenerTransacciones();
            this.resetForm();
          })
          .catch(err => {
            alert("Error al editar transacción.");
            console.error(err);
          });
      }
    },
    borrar(id) {
      if (confirm("¿Seguro que querés borrar esta transacción?")) {
        axios.delete(`${this.apiBase}/Transaccion/${id}`)
          .then(() => this.obtenerTransacciones());
      }
    },
    cancelarEdicion() {
      this.resetForm();
    },
    resetForm() {
      this.editando = false;
      this.form = {
        id: null,
        cryptoCode: "",
        action: "",
        cryptoAmount: null,
        money: 0,
        dateTime: ""
      };
    },
    formatFecha(fecha) {
      return new Date(fecha).toLocaleString();
    },
    nombreCripto(code) {
      const nombres = {
        btc: "Bitcoin",
        eth: "Ethereum",
        usdc: "USDC"
      };
      return nombres[code] || code;
    },
    nombreAccion(action) {
      return action === "purchase"
        ? "Compra"
        : action === "sale"
        ? "Venta"
        : action;
    },
    calcularAnalisis() {
      const resumen = {};
      this.totalCartera = 0;
      this.analisis = [];

      for (const tx of this.transacciones) {
        const code = tx.cryptoCode;
        if (!resumen[code]) resumen[code] = 0;
        resumen[code] += tx.action === "purchase"
          ? tx.cryptoAmount
          : -tx.cryptoAmount;
      }

      const promesas = Object.entries(resumen)
        .filter(([_, cantidad]) => cantidad > 0)
        .map(([code, cantidad]) => {
          return axios.get(`https://criptoya.com/api/satoshitango/${code}/ars`)
            .then(resp => {
              const precio = resp.data.totalBid;
              const valorActual = precio * cantidad;
              this.totalCartera += valorActual;
              this.analisis.push({ code, cantidad, valorActual });
            })
            .catch(err => console.error("Error CriptoYa", code, err));
        });

      Promise.all(promesas).then(() => this.dibujarGrafico());
    },
    dibujarGrafico() {
      const ctx = document.getElementById('graficoCartera');
      if (!ctx) return;

      const data = {
        labels: this.analisis.map(item => this.nombreCripto(item.code)),
        datasets: [{
          label: 'Valor en ARS',
          data: this.analisis.map(item => item.valorActual),
          backgroundColor: ['#36a2eb', '#ff6384', '#ffcd56'],
        }]
      };

      if (this.grafico) this.grafico.destroy();

      this.grafico = new Chart(ctx, {
        type: 'pie',
        data: data
      });
    },
    mostrarComprobante(tx) {
      this.comprobanteActual = tx;
      this.vista = 'comprobantes';
    }
  },
  mounted() {
    this.obtenerTransacciones();
  }
}).mount("#app");
