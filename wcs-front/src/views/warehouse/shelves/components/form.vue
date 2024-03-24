<template>
  <div>
    <a-modal v-model="visible" :title="title" @ok="handleOk" :maskClosable="false">
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
        >
          <a-form-model-item label="货架编号" prop="shelves_code">
            <a-input v-model="form.shelves_code" placeholder="请填写货架编号" />
          </a-form-model-item>
          <a-form-model-item label="货架列数" prop="shelves_cols_num">
            <a-input v-model="form.shelves_cols_num" placeholder="请填写货架列数" type="number" addon-after="列" />
          </a-form-model-item>
          <a-form-model-item label="货架层数" prop="shelves_rows_num">
            <a-input v-model="form.shelves_rows_num" placeholder="请填写货架层数" type="number" addon-after="层" />
          </a-form-model-item>
          <a-form-model-item label="货架深度" prop="shelves_deep_num">
            <a-input v-model="form.shelves_deep_num" placeholder="请填写货架深度" type="number" addon-after="深" />
          </a-form-model-item>
          <a-form-model-item label="单格最大承重量" prop="max_load">
            <a-input v-model="form.max_load" placeholder="请填写单格最大承重量" type="number" addon-after="KG" />
          </a-form-model-item>

          <a-form-model-item label="起始x值" prop="point_x">
            <a-input v-model="form.point_x" placeholder="起始x值" type="number" addon-after="mm" />
          </a-form-model-item>

          <a-form-model-item label="起始z值" prop="point_z">
            <a-input v-model="form.point_z" placeholder="起始z值" type="number" addon-after="mm" />
          </a-form-model-item>

          <a-form-model-item label="单格货位高度" prop="point_height">
            <a-input v-model="form.point_height" placeholder="单格货位高度" type="number" addon-after="mm" />
          </a-form-model-item>

          <a-form-model-item label="关联设备" prop="relation_object_id">
            <a-select v-model="form.relation_object_id" placeholder="请选择设备">
              <a-select-option v-for="item in equipmentList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="库区" prop="warehouse_area">
            <a-select v-model="form.warehouse_area" placeholder="请选择设备所属库区">
              <a-select-option v-for="item in warehouseAreaList" :key="item.value" :value="item.value">{{
                item.label
              }}</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="上游系统ID" prop="upper_system_id">
            <a-input v-model="form.upper_system_id" placeholder="上游系统ID" disabled />
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </a-modal>
  </div>
</template>
<script>
import { getOptions, getAllEquipment } from '@/api/wcs/equipment'
import { update, getDetailById } from '@/api/wcs/shelves'

export default {
  name: 'ShelvesForm',

  data() {
    return {
      visible: false,
      loading: false,
      title: '货架录入',

      labelCol: { span: 6 },
      wrapperCol: { span: 16 },
      form: {
        id: '',
        shelves_code: '',
        shelves_cols_num: '',
        shelves_rows_num: '',
        shelves_deep_num: '',
        max_load: '',
        upper_system_id: '',
        warehouse_area: undefined,
        relation_object_id: undefined,
        point_x: '',
        point_y: '',
        point_z: '',
        point_height: '',
        point_width: ''
      },
      rules: {
        shelves_code: [{ required: true, message: '请填写货架编号' }],
        shelves_cols_num: [{ required: true, message: '请填写货架列数' }],
        shelves_rows_num: [{ required: true, message: '请填写货架层数' }],
        shelves_deep_num: [{ required: true, message: '请填写货架深度' }],
        max_load: [{ required: true, message: '请填写货架单格最大承重量' }],
        point_x: [{ required: true, message: '请填写起始x值' }],
        point_z: [{ required: true, message: '请填写起始z值' }],
        point_height: [{ required: true, message: '请填写单格货位高度' }]
      },

      equipmentList: [],
      warehouseAreaList: []
    }
  },
  methods: {
    showModal(id) {
      console.log('showModal' + id)
      this.title = id ? '货架编辑' : '货架录入'
      this.form.id = id
      this.visible = true
      if (this.$refs.ruleForm) this.$refs.ruleForm.resetFields()
      this.getDefaultData()
    },
    handleOk(e) {
      if (e === 'cancel') {
        this.visible = false
        return
      }

      this.$refs.ruleForm.validate((valid) => {
        if (valid) {
          this.loading = true

          update({
            ...this.form
          }).then((res) => {
            if (res.code === 200) {
              this.visible = false
              this.loading = false
              this.$message.success('操作成功')
              this.$emit('onFinish', { type: e, data: this.form })
            }
          })
        } else {
          console.log('error submit!!')
          return false
        }
      })
    },
    getDefaultData() {
      getOptions().then((res) => {
        if (res.code === 200) {
          this.warehouseAreaList = res.data.wareHouse.map((item) => {
            return {
              value: item.id,
              label: item.warehouse_name
            }
          })
        }
      })

      getAllEquipment({ type: '' }).then((res) => {
        if (res.code === 200) {
          this.equipmentList = res.data.equipments.map((item) => {
            return {
              value: item.id,
              label: item.equipment_name
            }
          })
        }
      })

      if (!this.form.id) return
      this.loading = true
      getDetailById({ id: this.form.id }).then((res) => {
        if (res.code === 200) {
          this.form = { ...res.data }
        }
        this.loading = false
      })
    }
  }
}
</script>
