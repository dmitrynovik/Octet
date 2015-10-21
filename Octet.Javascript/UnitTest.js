
QUnit.test("'a' startsWith 'a'", function (assert) {
    assert.ok("a".startsWith("a"));
});

QUnit.test("'a' startsWith empty string", function (assert) {
    assert.ok("a".startsWith(""));
});

QUnit.test("'a' startsWith null", function (assert) {
    assert.ok("a".startsWith(null));
});

QUnit.test("'ab' does not start with 'b'", function (assert) {
    assert.notOk("a".startsWith("b"));
});

QUnit.test("'ab' endsWith 'b'", function (assert) {
    assert.ok("ab".endsWith("b"));
});

QUnit.test("'a' endsWith empty string", function (assert) {
    assert.ok("a".endsWith(""));
});

QUnit.test("'a' endsWith null", function (assert) {
    assert.ok("a".endsWith(null));
});

QUnit.test("'ab' does not end with 'a'", function (assert) {
    assert.notOk("ab".endsWith("a"));
});

QUnit.test("strip Html", function (assert) {
    assert.ok("<head><meta charset='utf-'><title>QUnit Example</title></head>".stripHtml() === "QUnit Example");
});


