<template>
  <global-footer class="footer custom-render">
    <template v-slot:links>
      <div>foot</div>
    </template>
    <template v-slot:copyright>
      <div>foot</div>
    </template>
  </global-footer>
</template>

<script>
import { GlobalFooter } from '@/components/ProLayout'

export default {
  name: 'ProGlobalFooter',
  components: {
    GlobalFooter
  }
}
</script>
