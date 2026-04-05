box.cfg{
    listen = 3301,
    memtx_memory = 2 * 1024 * 1024 * 1024
}
box.schema.space.create('KV', {
    if_not_exists = true,
    format = {
        {name = 'key', type = 'string'},
        {name = 'value', type = 'varbinary', is_nullable = true}
    }
})

box.space.KV:create_index('primary', {
    parts = {'key'},
    if_not_exists = true
})

box.schema.user.grant('guest', 'read,write,execute', 'universe', nil, {if_not_exists = true})