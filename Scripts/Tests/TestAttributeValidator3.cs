using AttributeValidation;
using UnityEngine;
using static AttributeValidation.PathValidator;

#pragma warning disable 0169
#pragma warning disable 0649
#pragma warning disable CA1823

public class TestAttributeValidator3 : MonoBehaviour
{
    [SerializeField, NotNull, PathValidation(PathKind.Relative)]
    private string m_path1_relative_file_good;

    [SerializeField, NotNull, PathValidation(PathKind.Relative)]
    private string m_path1_relative_file_bad;

    [SerializeField, NotNull, PathValidation(PathKind.Absolute)]
    private string m_path2_absolute_file_good;

    [SerializeField, NotNull, PathValidation(PathKind.Absolute)]
    private string m_path2_absolute_file_bad;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, "", ".png")]
    private string m_path3_relative_file_good_extension;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, "", ".jpg")]
    private string m_path3_relative_file_bad_extension;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, "_PROJECT\\")]
    private string m_path4_starting_path_good;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, "Toto/")]
    private string m_path4_starting_path_bad;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, PathValidator.PathForObjectKind.Folder)]
    private string m_path5_is_dir_good;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, PathValidator.PathForObjectKind.Folder)]
    private string m_path5_is_dir_bad;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, PathValidator.PathForObjectKind.File)]
    private string m_path6_is_file_good;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, PathValidator.PathForObjectKind.File)]
    private string m_path6_is_file_bad;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, PathValidator.PathForObjectKind.Any)]
    private string m_path7_is_file_or_dir_good_0;

    [SerializeField, NotNull, PathValidation(PathValidator.PathKind.Relative, PathValidator.PathForObjectKind.Any)]
    private string m_path7_is_file_or_dir_good_1;
}

#pragma warning restore 0169
#pragma warning restore 0649
#pragma warning restore CA1823