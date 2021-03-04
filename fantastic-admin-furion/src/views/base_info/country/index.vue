<template>
    <div class="page-main">
        <!-- head -->
        <SimpleHead
            :model="filter"
            @on-search="reloadTable(true)"
            @on-expand="onExpand"
        >
            <template slot="button">
                <!-- 操作按钮 -->
                <el-form-item>
                    <el-button-group>
                        <el-button
                            type="success"
                            icon="el-icon-plus"
                            @click="add"
                        >
                            添加
                        </el-button>
                    </el-button-group>
                </el-form-item>
            </template>
            <!-- 默认搜索项 -->
            <el-form-item>
                <el-input
                    v-model="filter.name"
                    placeholder="国家名称"
                    clearable
                />
            </el-form-item>
            <el-form-item>
                <el-input
                    v-model="filter.code"
                    placeholder="国家简码"
                    clearable
                />
            </el-form-item>
            <template slot="more">
                <!-- 更多搜索项 -->
            </template>
        </SimpleHead>
        <!-- table -->
        <SimpleTable ref="table" :url="tableUrl" :filter="filter" :offset="10">
            <el-table-column type="selection" width="40" fixed="left" />
            <el-table-column
                prop="continent"
                label="大洲"
                width="100"
            />
            <el-table-column prop="enName" label="英文名称" />
            <el-table-column prop="cnName" label="简体中文" />
            <el-table-column prop="twName" label="繁体中文" />
            <el-table-column prop="twoCode" label="二字码" />
            <el-table-column prop="threeCode" label="三字码" />
            <el-table-column
                prop="otherCode"
                label="其它简码"
            />
            <el-table-column prop="remark" label="备注" />
            <el-table-column prop="operate" label="操作" width="80">
                <template slot-scope="scope">
                    <el-button-group>
                        <el-button
                            type="primary"
                            icon="el-icon-edit"
                            circle
                            size="mini"
                            title="编辑"
                            @click="edit(scope.row)"
                        />
                        <el-button
                            type="danger"
                            icon="el-icon-delete"
                            circle
                            size="mini"
                            title="删除"
                            @click="remove(scope.row)"
                        />
                    </el-button-group>
                </template>
            </el-table-column>
        </SimpleTable>
        <!-- 添加编辑 -->
        <AddEdit ref="edit" @on-ok="reloadTable" />
    </div>
</template>
<script>
export default {
    name: 'Country',
    components: {
        AddEdit: () => import('./addedit') // 添加编辑
    },
    data() {
        return {
            tableUrl: `${process.env.VUE_APP_API_ROOT}api/baseinfo/getcountrylist`,
            // 过滤项
            filter: {
                name: null,
                code: null
            }
        }
    },
    methods: {
        reloadTable(again) {
            this.$refs.table.reload(again)
        },
        onExpand() {
            this.$refs.table.resize()
        },
        add() {
            this.$refs.edit.open()
        },
        edit(row) {
            this.$refs.edit.open(row)
        },
        remove(row) {
            let ids = []
            if (!row) {
                const rows = this.$refs.table.getSelection()
                if (rows.length === 0) {
                    this.$message.warning('请选择要删除的数据')
                    return
                }
                ids = rows.map(e => e.userId)
            }

            this.$confirm('确定要删除国家吗？', '删除', {
                distinguishCancelAndClose: true,
                confirmButtonText: '确定',
                cancelButtonText: '取消'
            })
                .then(() => {
                    this.$api.post('api/baseinfo/countryemove', ids).then(res => {
                        if (res.succeeded) {
                            this.$message.success('删除成功.')
                            this.reloadTable()
                        } else {
                            this.$message.error(res.errors)
                        }
                    })
                })
                .catch(error => {
                    console.log(error)
                })
        }
    }
}
</script>
