<template>
  <transition name="modal-fade">
    <div class="modal-backdrop">
      <div
        class="modal"
        role="dialog"
        aria-labelledby="modalTitle"
        aria-describedby="modalDescription"
      >
        <header class="modal-header" id="modalTitle">
          <slot name="header"> Default Header </slot>
          <!-- <button
            type="button"
            class="close-btn"
            @click="close"
            aria-label="Close Modal"
          >
            x 
          </button>-->
        </header>

        <main class="modal-body" id="modalDescription">
          <slot name="body"> Default body content </slot>
        </main>

        <footer class="modal-footer">
          <slot name="footer"> Default Footer! </slot>
          <button
            type="button"
            class="btn-open-modal"
            @click="close"
            aria-label="Close Modal"
          >
            Close
          </button>
        </footer>
      </div>
    </div>
  </transition>
</template>

<script>
export default {
  name: "Modal",
  methods: {
    close() {
      this.$emit("close");
    },
  },
};
</script>
<style>
.modal-backdrop {
  position: fixed;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background-color: rgba(0, 0, 0, 0.3);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal {
  background-image: linear-gradient(to bottom right, #7f0cf2, #ff00dc);
  color: white;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow-x: auto;
  display: flex;
  flex-direction: column;
  border-radius: 10px;
  margin: auto;
}

.modal-header,
.modal-footer {
  padding: 15px;
  display: flex;
}

.modal-header {
  position: relative;
  border-bottom: 1px solid rgb(227, 231, 233);
  font-weight: bold;
  justify-content: space-between;
}

.modal-footer {
  border-top: 1px solid rgb(227, 231, 233);
  flex-direction: column;
  justify-content: flex-end;
  text-align: center;
}

.modal-body {
  position: relative;
  padding: 20px 10px;
}

.close-btn {
  position: absolute;
  top: 0;
  right: 0;
  border: none;
  font-size: 20px;
  padding: 10px;
  cursor: pointer;
  font-weight: bold;
  color: red;
  background: transparent;
  
}

.btn-open-modal {
  color: white;
  background: #ffc700;
  color: black;
  border: 1px solid #ffc700;;
  border-radius: 4px;
  margin: 20px auto;
  padding: 5px;
  width: 40%;
  cursor: pointer;
}

.modal-fade-enter,
.modal-fade-leave-to {
  opacity: 0;
}

.modal-fade-enter-active,
.modal-fade-leave-active {
  transition: opacity 0.5s ease;
}

@media screen and (max-width: 768px) {
  .modal {
    width: 90%;
    height: 150%;
  }
}
</style>