import { useEffect, useState } from "react";
import type { BookSort } from "../../types";
import "@/assets/styles/components/book/catalog-sort.css"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCaretDown, faCaretUp } from "@fortawesome/free-solid-svg-icons";
interface CatalogSortProps {
    onSortChange: (field: keyof BookSort) => void
    sort: BookSort
}

const CatalogSort: React.FC<CatalogSortProps> = ({ onSortChange, sort }) => {
    const [expanded, setExpanded] = useState<boolean>(false);
    const [selectedOption, setSelectedOption] = useState<string>("newest");
    const [optionState, setOptionState] = useState<boolean | undefined>(true);
    const toggleExpanded = () => setExpanded(!expanded);



    const handleSort = (option: keyof BookSort) => {
        if (option == selectedOption) {
            setOptionState(!optionState)
        }
        else {
            setSelectedOption(option);
            setOptionState(true);
        }
        onSortChange(option);
    }

    // Temporary
    useEffect(() => {
        handleSort("newest");
    }, [])

    return (
        <div className="relative">
            {/* Toggle Button */}
            <button
                className="bg-[#F4F0E5] border-none px-4 py-2 w-48 text-start"
                onClick={() => toggleExpanded()}
            >
                By {selectedOption}
                <span className=" float-right">
                    {optionState !== undefined ?
                        <FontAwesomeIcon icon={optionState ? faCaretUp : faCaretDown} />
                        : ""
                    }
                </span>
            </button>

            {/* Animated Dropdown Panel */}
            <div
                className={`sort-container ${expanded ? "max-h-[500px] opacity-100" : "max-h-0 opacity-0"
                    }`}
            >
                <div className="flex flex-col p-2">
                    <button className="sort-btn" onClick={() => handleSort("newest")}>
                        <p className="sort-title">
                            By newest
                            <span className=" float-right">
                                {sort.newest !== undefined ?
                                    <FontAwesomeIcon icon={sort.newest ? faCaretUp : faCaretDown} />
                                    : ""
                                }
                            </span>
                        </p>
                    </button>
                    <button className="sort-btn" onClick={() => handleSort("title")}>
                        <p className="sort-title">By title
                            <span className=" float-right">
                                {sort.title !== undefined ?
                                    <FontAwesomeIcon icon={sort.title ? faCaretUp : faCaretDown} />
                                    : ""
                                }
                            </span>
                        </p>
                    </button>
                    <button className="sort-btn" onClick={() => handleSort("price")}>
                        <p className="sort-title">By price
                            <span className=" float-right">
                                {sort.price !== undefined ?
                                    <FontAwesomeIcon icon={sort.price ? faCaretUp : faCaretDown} />
                                    : ""
                                }
                            </span>
                        </p>
                    </button>
                    <button className="sort-btn" onClick={() => handleSort("year")}>
                        <p className="sort-title">By year
                            <span className=" float-right">
                                {sort.year !== undefined ?
                                    <FontAwesomeIcon icon={sort.year ? faCaretUp : faCaretDown} />
                                    : ""
                                }
                            </span>
                        </p>
                    </button>
                    <button className="sort-btn" onClick={() => handleSort("feedBack")}>
                        <p className="sort-title">By feedback
                            <span className=" float-right">
                                {sort.feedBack !== undefined ?
                                    <FontAwesomeIcon icon={sort.feedBack ? faCaretUp : faCaretDown} />
                                    : ""
                                }
                            </span>
                        </p>
                    </button>
                    <button className="sort-btn" onClick={() => handleSort("discountRate")}>
                        <p className="sort-title">By discount
                            <span className=" float-right">
                                {sort.discountRate !== undefined ?
                                    <FontAwesomeIcon icon={sort.discountRate ? faCaretUp : faCaretDown} />
                                    : ""
                                }
                            </span>
                        </p>
                    </button>
                </div>
            </div>
        </div>
    );
}

export default CatalogSort