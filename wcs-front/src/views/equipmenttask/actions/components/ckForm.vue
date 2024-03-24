<template>
  <div>
    <a-modal v-model="visible" :title="title" @ok="handleOk">
      <template slot="footer">
        <a-button key="cancel" @click="handleOk('cancel')">取消</a-button>
        <a-button key="submit" type="primary" :loading="loading" @click="handleOk('submit')">确定</a-button>
      </template>
      <a-spin tip="Loading..." :spinning="loading">
        <a-form-model
          ref="ruleForm"
          :model="form"
          :rules="rules"
          :label-col="labelCol"
          :wrapper-col="wrapperCol"
          :loading="true"
        >
          <a-divider>第一步：选择载货货位</a-divider>
          <a-form-model-item label="货架" prop="shelves_id">
            <a-select v-model="form.shelves_id" placeholder="请选货架">
              <a-select-option v-for="item in shelvesList" :key="item.value" :value="item.value">
                {{ item.label }}
              </a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="货位" prop="shelves_position_name">
            <a-input
              class="select-hw"
              v-model="form.shelves_position_name"
              placeholder="请选择货位"
              @click="onSelectHw"
              addon-after="(x,y,z)"
              :disabled="!this.form.shelves_id"
            />
          </a-form-model-item>

          <a-divider>第二步：选择出库输送线</a-divider>
          <a-form-model-item label="设备类型">
            <a-input value="输送系统" disabled />
          </a-form-model-item>
          <a-form-model-item label="设备名称" prop="equipment_id">
            <a-select v-model="form.equipment_id" placeholder="请选择设备" :disabled="!this.form.shelves_id">
              <a-select-option v-for="item in equipmentList" :key="item.value" :value="item.value">
                {{ item.label }}
              </a-select-option>
            </a-select>
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </a-modal>

    <shelves-position ref="ShelvesPosition" :type="'ck'" @onFinish="onFinishSelectHw" />
  </div>
</template>
<script>
import { getAllEquipmentsManual } from '@/api/wcs/equipment'
import { selctListForManual } from '@/api/wcs/shelves'
import { manualCkMode } from '@/api/wcs/task'

import ShelvesPosition from '@/components/ShelvesPosition'

export default {
  name: 'CkForm',
  components: { ShelvesPosition },
  data() {
    return {
      visible: false,
      loading: false,
      title: '手工出库',

      labelCol: { span: 6 },
      wrapperCol: { span: 16 },
      form: {
        shelves_id: undefined,
        shelves_position_id: '',
        shelves_position_name: '',
        equipment_id: undefined
      },
      rules: {
        shelves_id: [{ required: true, message: '请选择货架' }],
        equipment_id: [{ required: true, message: '请选择设备' }],
        shelves_position_name: [{ required: true, message: '请选择货位' }]
      },
      equipmentTypeList: [
        { id: '99ae0692-85b3-11ed-a5d9-00163e10b48b', equipment_type_name: '输送系统' },
        { id: 'cfee1e01-7ddd-11ed-a5d9-00163e10b48b', equipment_type_name: '堆垛机' },
        { id: '7ad3a259-7ddd-11ed-a5d9-00163e10b48b', equipment_type_name: '托盘AGV' }
      ],
      ssxType: '99ae0692-85b3-11ed-a5d9-00163e10b48b',
      equipmentList: [],
      shelvesList: []
    }
  },
  watch: {
    'form.shelves_id': {
      handler(val) {
        if (val) {
          this.form.equipment_id = undefined
          this.getEquipments(this.ssxType, val)
        }
      },
      immediate: true
    }
  },
  methods: {
    showModal() {
      this.visible = true
      if (this.$refs.ruleForm) this.$refs.ruleForm.resetFields()
      this.getShelves('')
    },
    handleOk(e) {
      if (e === 'cancel') {
        this.visible = false
        return
      }

      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          this.loading = true
          manualCkMode({ startObject: this.form.shelves_position_id, endObject: this.form.equipment_id }).then(
            (res) => {
              this.loading = false
              if (res.data && res.status) {
                if (res.data.status) {
                  this.$message.success('操作成功')
                  this.visible = false
                } else {
                  this.$message.error(res.data.message)
                }
              } else {
                this.$message.error(res.message)
              }
            }
          )
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    getEquipments(type, shelvesId) {
      getAllEquipmentsManual({ type, shelves_id: shelvesId }).then((res) => {
        if (res.code === 200) {
          this.equipmentList = res.data.equipments.map((item) => {
            return {
              value: item.id,
              label: item.equipment_name
            }
          })
        }
      })
    },
    getShelves(equipmentId) {
      selctListForManual({ equipmentId }).then((res) => {
        if (res.code === 200) {
          this.shelvesList = res.data.shelves.map((item) => {
            return {
              value: item.id,
              label: item.shelves_code
            }
          })
        }
      })
    },
    onSelectHw() {
      if (this.form.shelves_id) {
        this.$refs.ShelvesPosition.showModal(this.form.shelves_id)
      }
    },
    onFinishSelectHw(value) {
      if (value.data) {
        this.form.shelves_position_name = `${value.data.row}-${value.data.col}货位（${value.data.point_x},${value.data.point_y},${value.data.point_z}）`
        this.form.shelves_position_id = value.data.id
      }
    }
  }
}
</script>
<style lang="less">
.select-hw {
  .ant-input {
    cursor: pointer;
  }
}
</style>
