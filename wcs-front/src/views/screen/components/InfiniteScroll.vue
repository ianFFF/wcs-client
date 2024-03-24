<template>
  <div class="infinite-scroll-container">
    <div :style="`transform: translate(0px, ${translateX}px); transition: all 0ms ease-in 0s;`">
      <div style="overflow: hidden">
        <slot></slot>
      </div>
      <div style="overflow: hidden">
        <slot></slot>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      translateX: 0
    }
  },
  mounted() {
    const maxHeigth = -162
    setInterval(() => {
      if (maxHeigth >= this.translateX) {
        this.translateX = 0
      } else {
        this.translateX = this.translateX - 0.1
      }
    }, 6)
  }
}
</script>

<style></style>
